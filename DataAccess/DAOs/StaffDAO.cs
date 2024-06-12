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
using DataAccess.IRepository;

namespace DataAccess.DAOs
{
    public class StaffDAO
    {

        private readonly NirvaxContext _context;
        private readonly IMapper _mapper;
        private IFileService _fileService;



        public StaffDAO(IFileService fs, NirvaxContext context, IMapper mapper)
        {
            this._fileService = fs;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckStaff(StaffDTO staffDTO)
        {

            Staff? Staff = new Staff();
            Staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffDTO.StaffId);
            Staff? StaffCreate = new Staff();
            StaffCreate = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.Email == staffDTO.Email || i.Phone == staffDTO.Phone);
            if (Staff != null)
            {
                List<Staff> getList = await _context.Staff
                 
                 //check khác Id`
                 .Where(i => i.StaffId != staffDTO.StaffId)
                 .Where(i => i.Email == staffDTO.Email || i.Phone == staffDTO.Phone)
                 .ToListAsync();
                if (getList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            } else if (StaffCreate == null && Staff == null){
              
                return true;
            }
            return false;
        }

        public async Task<bool> CheckProfileStaff(StaffProfileDTO staffProfileDTO)
        {
            // StaffDTO checkownerDTO = new StaffDTO();
            Staff? Staff = new Staff();
            Staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffProfileDTO.StaffId);
            if (Staff != null)
            {
                List<Staff> getList = await _context.Staff
                
                 //check khác Id
                 .Where(i => i.StaffId != staffProfileDTO.StaffId)
                 .Where(i => i.Email == staffProfileDTO.Email || i.Phone == staffProfileDTO.Phone)
                 .ToListAsync();
                if (getList.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        //check xem owner đó có tồn tại hay không
        public async Task<bool> CheckStaffExist(int staffId)
        {
            Staff? sid = new Staff();

            sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);

            if (sid == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckProfileExist(string staffEmail)
        {
            Staff? sid = new Staff();

            sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.Email == staffEmail);

            if (sid == null)
            {
                return false;
            }
            return true;
        }



        public async Task<bool> ChangePasswordStaff(int staffId, string oldPassword, string newPasswod, string confirmPassword)
        {
            //check password             
            Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
            bool verified = BCrypt.Net.BCrypt.Verify(oldPassword, sid.Password);
            if (verified == true)
            {
                if(newPasswod == confirmPassword)
                {
                    string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(newPasswod);
                    sid.Password = newpasswordHash;
                    _context.Staff.Update(sid);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    
                       throw new Exception("Your new password and confirm password not match!");
                    
                }
               
            }
            else
            {

                throw new Exception("Enter wrong your old password!");

            }
        }


        //owner
        public async Task<List<StaffDTO>> GetAllStaffs(string? searchQuery, int page, int pageSize)
        {
            List<StaffDTO> listStaffDTO = new List<StaffDTO>();


            if (!string.IsNullOrEmpty(searchQuery))
            {
                List<Staff> getList = await _context.Staff.Include(i => i.Owner)
                    .Where(i => i.Fullname.Trim().Contains(searchQuery) || i.Email.Trim().Contains(searchQuery) || i.Phone.Trim().Contains(searchQuery))
                   
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<StaffDTO>>(getList);
            }
            else
            {
                List<Staff> getList = await _context.Staff.Include(i => i.Owner)
                
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                listStaffDTO = _mapper.Map<List<StaffDTO>>(getList);
            }
            return listStaffDTO;
        }

        //details
        public async Task<StaffDTO> GetStaffById(int staffId)
        {
            StaffDTO staffDTO = new StaffDTO();
            try
            {
                Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
                staffDTO = _mapper.Map<StaffDTO>(sid);
                return staffDTO;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
           
        }

        //profile
        public async Task<StaffDTO> GetStaffByEmail(string staffEmail)
        {
            StaffDTO staffDTO = new StaffDTO();
            try
            {
                Staff? sid = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.Email == staffEmail);
                staffDTO = _mapper.Map<StaffDTO>(sid);
                return staffDTO;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
           
        }



        public async Task<bool> CreateStaff(StaffDTO staffDTO)
        {
            
            string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(staffDTO.Password);
            staffDTO.Password = newpasswordHash;
            Staff staff = _mapper.Map<Staff>(staffDTO);


            await _context.Staff.AddAsync(staff);
            int i = await _context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else { return false; }

        }

        //admin

        //oldPass = xyz
        //newPass =1234
        public async Task<bool> UpdateStaff(StaffDTO staffDTO)
        {
            StaffDTO newStaff;
            newStaff = staffDTO;
            Staff? staffOrgin = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffDTO.StaffId);
            bool verified = BCrypt.Net.BCrypt.Verify(staffDTO.Password,staffOrgin.Password); //123 != ljkj
            if(verified == false)
            {
                string newpasswordHash = BCrypt.Net.BCrypt.HashPassword(staffDTO.Password);
                staffDTO.Password = newpasswordHash;
            } else
            {
                staffDTO.Password = staffOrgin.Password;
            }
              // staffOrgin.Image = staffDTO.
             
                _mapper.Map(staffDTO, staffOrgin);
                 _context.Staff.Update(staffOrgin);
                await _context.SaveChangesAsync();
                return true;
           

        }

        //owner
        public async Task<bool> UpdateProfileStaff(StaffProfileDTO staffProfileDTO)
        {
            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffProfileDTO.StaffId);


            staff.OwnerId = staff.OwnerId;
            staff.Image = staff.Image;
            staff.Password = staff.Password;
           
            _mapper.Map(staffProfileDTO, staff);
                
                 _context.Staff.Update(staff);
                await _context.SaveChangesAsync();
                return true;
          

        }
        public async Task<bool> UpdateAvatarStaff(StaffAvatarDTO staffAvatarDTO)
        {
           
            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffAvatarDTO.StaffId);
           

            _mapper.Map(staffAvatarDTO, staff);
             _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task<bool> BanStaff(int staffId)
        {
            Staff? staff = await _context.Staff.Include(i => i.Owner).SingleOrDefaultAsync(i => i.StaffId == staffId);
            //ánh xạ đối tượng staffdto đc truyền vào cho Staff
            if (staff != null)
            {
           
                 _context.Staff.Update(staff);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

            
            

        }
        
    }
}





