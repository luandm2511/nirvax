using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Azure;
using Azure.Core;
using System.Security.Cryptography;
using System.Drawing;
using Microsoft.Identity.Client;
using Pipelines.Sockets.Unofficial.Buffers;

namespace DataAccess.DAOs
{
    public  class RoomDAO
    {

        private readonly NirvaxContext  _context;
        private readonly IMapper _mapper;




        public  RoomDAO(NirvaxContext context, IMapper mapper)
        {

             _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckRoomAsync(int accountId, int ownerId)
        {

            List<Account> listAccount = await _context.Accounts.Where(i=> i.AccountId == accountId).ToListAsync();
            List<Owner> listOwner = await _context.Owners.Where(i => i.OwnerId == ownerId).ToListAsync();


            if (listAccount.Count > 0 && listOwner.Count > 0)
            {
                    return true;           
            }
          
                return false;
        }

       

        //owner,staff
        public async Task<IEnumerable<RoomDTO>> ViewUserHistoryChatAsync(int accountId)
        {
            var checkRoom = await _context.Accounts.Where(i => i.AccountId == accountId).FirstOrDefaultAsync();
            if (checkRoom == null) { return new List<RoomDTO>(); }
            var listRoomDTO = await _context.Rooms
            .Include(i => i.Account)
            .Include(i => i.Owner)
            .Select(room => new
        {
            Room = room,
            LatestMessageTimestamp = room.Messages.OrderByDescending(m => m.Timestamp).Select(m => m.Timestamp).FirstOrDefault()
        })
        .Where(x => x.Room.Account.AccountId == accountId)
        .OrderByDescending(x => x.LatestMessageTimestamp)
        .Select(x => x.Room)
        .ToListAsync();

            var roomDTOs = _mapper.Map<List<RoomDTO>>(listRoomDTO);

            return roomDTOs;
        }

        public async Task<IEnumerable<RoomDTO>> ViewOwnerHistoryChatAsync(int ownerId)
        {
            var checkRoom = await _context.Owners.Where(i => i.OwnerId == ownerId).FirstOrDefaultAsync();
            if (checkRoom == null) { throw new Exception("Not exist this user!"); };
            var listRoomDTO = await _context.Rooms
           .Include(i => i.Account)
           .Include(i => i.Owner)
           .Select(room => new
           {
               Room = room,
               LatestMessageTimestamp = room.Messages.OrderByDescending(m => m.Timestamp).Select(m => m.Timestamp).FirstOrDefault()
           })
       .Where(i => i.Room.Owner.OwnerId == ownerId)
       .OrderByDescending(x => x.LatestMessageTimestamp)
       .Select(x => x.Room)
       .ToListAsync();

            var roomDTOs = _mapper.Map<List<RoomDTO>>(listRoomDTO);

            return roomDTOs;
        }

        public async Task<RoomDTO> GetRoomByAccountIdAndOwnerIdAsync(int accountId, int ownerId)
        {
                RoomDTO roomDTO = new RoomDTO();
                Room? sid = await _context.Rooms
                    .Include(i => i.Account)
                    .Include(i => i.Owner)
                    .SingleOrDefaultAsync(i => i.AccountId == accountId && i.OwnerId == ownerId);             
                roomDTO = _mapper.Map<RoomDTO>(sid);
                return roomDTO;
        }

        public async Task<int> GetRoomIdByAccountIdAndOwnerIdAsync(int accountId, int ownerId) 
        {
                var roomId = 0;
                Room? sid = await _context.Rooms
                    .Include(i => i.Account)
                    .Include(i => i.Owner)
                    .SingleOrDefaultAsync(i => i.AccountId == accountId && i.OwnerId == ownerId);
                if(sid == null)
                {
                return 0;
                }
                roomId = _mapper.Map<RoomDTO>(sid).RoomId;
                return roomId;
        }

        public async Task<bool> CheckRoomExistAsync(int roomId)
        {
            Room? sid = new Room();

            sid = await _context.Rooms.Include(i => i.Account)
                    .Include(i => i.Owner).SingleOrDefaultAsync(i => i.RoomId == roomId); ;

            if (sid == null)
            {
                return false;
            }
            return true;
        }

        public async Task<RoomDTO> GetRoomByIdAsync(int roomId)
        {
            RoomDTO roomDTO = new RoomDTO();
                Room? sid = await _context.Rooms.Include(i => i.Account)
                    .Include(i => i.Owner).SingleOrDefaultAsync(i => i.RoomId == roomId);
                roomDTO = _mapper.Map<RoomDTO>(sid);
                return roomDTO;
        }



        public async Task<Room> CreateRoomAsync(RoomCreateDTO roomCreateDTO)
        {
            Room room = await _context.Rooms.Where(i => i.OwnerId == roomCreateDTO.OwnerId && i.AccountId == roomCreateDTO.AccountId).FirstOrDefaultAsync();
            if(room == null)
            {
                roomCreateDTO.Timestamp = DateTime.Now;
                Room roomCreate = _mapper.Map<Room>(roomCreateDTO);
                 await _context.Rooms.AddAsync(roomCreate);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return roomCreate;
                }
                else
                {
                    throw new Exception("Failed to create a new room."); ;
                }
            }
            else
            {
                throw new Exception("Room between this owner and this user is already created");
            }

        }

        public async Task<bool> UpdateContentRoomAsync(int roomId)
        {
            Room room = await _context.Rooms.Where(i => i.RoomId == roomId).FirstOrDefaultAsync();
            if (room != null)
            {
                Message mess = await _context.Messages.Where(i => i.RoomId == roomId).OrderBy(i => i.Timestamp).LastOrDefaultAsync();
                room.Content = mess.Content;
                room.OwnerId = room.OwnerId;
                room.AccountId = room.AccountId;
                room.Timestamp = room.Timestamp;

                _context.Rooms.Update(room);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Room between this owner and this user is already created");
            }

        }




    }
}





