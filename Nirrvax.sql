USE [Nirvax]
GO
/****** Object:  Table [dbo].[AccessLogs]    Script Date: 7/16/2024 1:42:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessLogs](
	[access_id] [int] IDENTITY(1,1) NOT NULL,
	[access_time] [datetime] NOT NULL,
	[ip_address] [varchar](50) NOT NULL,
	[user_agent] [varchar](255) NOT NULL,
 CONSTRAINT [PK_AccessLogs] PRIMARY KEY CLUSTERED 
(
	[access_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[account_id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [char](60) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[image] [varchar](max) NULL,
	[phone] [varchar](10) NOT NULL,
	[dob] [date] NOT NULL,
	[gender] [varchar](6) NOT NULL,
	[address] [nvarchar](150) NOT NULL,
	[role] [varchar](5) NOT NULL,
	[is_ban] [bit] NOT NULL,
 CONSTRAINT [PK__Account__46A222CD0D8015C3] PRIMARY KEY CLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Advertisement]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Advertisement](
	[ad_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[content] [text] NOT NULL,
	[image] [varchar](max) NOT NULL,
	[status_post_id] [int] NOT NULL,
	[service_id] [int] NOT NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK__Advertis__CAA4A62737D7E1DD] PRIMARY KEY CLUSTERED 
(
	[ad_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[brand_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[image] [varchar](max) NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK__Brand__5E5A8E274E00B600] PRIMARY KEY CLUSTERED 
(
	[brand_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[image] [varchar](max) NOT NULL,
	[isdelete] [bit] NOT NULL,
	[cate_parent_id] [int] NOT NULL,
 CONSTRAINT [PK__Category__D54EE9B46EDD1701] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryParent]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryParent](
	[cate_parent_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK_CategoryParent] PRIMARY KEY CLUSTERED 
(
	[cate_parent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[comment_id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[owner_id] [int] NULL,
	[content] [nvarchar](200) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[reply] [nvarchar](200) NULL,
	[reply_timestamp] [datetime] NULL,
 CONSTRAINT [PK__Comment__E795768716814496] PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Description]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Description](
	[description_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[content] [text] NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK_Description] PRIMARY KEY CLUSTERED 
(
	[description_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestConsultation]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestConsultation](
	[guest_id] [int] IDENTITY(1,1) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[phone] [varchar](10) NOT NULL,
	[content] [nvarchar](500) NOT NULL,
	[status_guest_id] [int] NOT NULL,
	[ad_id] [int] NOT NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK__GuestCon__19778E3531EC48AB] PRIMARY KEY CLUSTERED 
(
	[guest_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GuestStatus]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GuestStatus](
	[status_guest_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__GuestSta__1ACF0575AC6339CC] PRIMARY KEY CLUSTERED 
(
	[status_guest_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[link_image] [varchar](max) NOT NULL,
	[isdelete] [bit] NOT NULL,
	[product_id] [int] NULL,
	[description_id] [int] NULL,
 CONSTRAINT [PK_Image_1] PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportProduct]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportProduct](
	[import_id] [int] IDENTITY(1,1) NOT NULL,
	[warehouse_id] [int] NOT NULL,
	[import_date] [datetime] NOT NULL,
	[origin] [nvarchar](50) NOT NULL,
	[quantity] [int] NOT NULL,
	[total_price] [float] NOT NULL,
 CONSTRAINT [PK__ImportPr__F3E6B05F2EFB601C] PRIMARY KEY CLUSTERED 
(
	[import_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImportProductDetail]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportProductDetail](
	[import_id] [int] NOT NULL,
	[product_size_id] [nvarchar](30) NOT NULL,
	[quantity_received] [int] NOT NULL,
	[unit_price] [float] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[message_id] [int] IDENTITY(1,1) NOT NULL,
	[sender_id] [int] NOT NULL,
	[receiver_id] [int] NULL,
	[content] [nvarchar](500) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[room_id] [int] NOT NULL,
 CONSTRAINT [PK__Message__0BBF6EE65EE8AE97] PRIMARY KEY CLUSTERED 
(
	[message_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notification_id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NULL,
	[owner_id] [int] NULL,
	[content] [nvarchar](200) NOT NULL,
	[is_read] [bit] NOT NULL,
	[url] [varchar](100) NULL,
	[create_date] [datetime] NOT NULL,
 CONSTRAINT [PK__Notifica__E059842FE7BF43ED] PRIMARY KEY CLUSTERED 
(
	[notification_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[code_order] [nvarchar](10) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[order_date] [datetime] NOT NULL,
	[shipped_date] [datetime] NULL,
	[required_date] [datetime] NULL,
	[phone] [varchar](10) NOT NULL,
	[address] [nvarchar](150) NOT NULL,
	[note] [nvarchar](200) NOT NULL,
	[total_amount] [float] NOT NULL,
	[account_id] [int] NOT NULL,
	[owner_id] [int] NOT NULL,
	[status_id] [int] NOT NULL,
	[voucher_id] [varchar](8) NULL,
 CONSTRAINT [PK__Order__46596229DEBCF00D] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[order_id] [int] NOT NULL,
	[product_size_id] [nvarchar](30) NOT NULL,
	[quantity] [int] NOT NULL,
	[unit_price] [float] NOT NULL,
 CONSTRAINT [PK_Multi] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[product_size_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[status_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__OrderSta__3683B531F1692557] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Owner]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Owner](
	[owner_id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [char](60) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[image] [varchar](max) NULL,
	[phone] [varchar](10) NOT NULL,
	[address] [nvarchar](200) NOT NULL,
	[is_ban] [bit] NOT NULL,
 CONSTRAINT [PK__Owner__AD081786280DDB20] PRIMARY KEY CLUSTERED 
(
	[owner_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostStatus]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostStatus](
	[status_post_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__PostStat__0271DBBF8F3DF3F0] PRIMARY KEY CLUSTERED 
(
	[status_post_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[short_description] [nvarchar](200) NOT NULL,
	[price] [float] NOT NULL,
	[quantity_sold] [int] NOT NULL,
	[rate_point] [float] NOT NULL,
	[rate_count] [int] NOT NULL,
	[isdelete] [bit] NOT NULL,
	[isban] [bit] NOT NULL,
	[description_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	[brand_id] [int] NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK__Product__47027DF5AC8E29A4] PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductSize]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductSize](
	[product_size_id] [nvarchar](30) NOT NULL,
	[size_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK__ProductS__062A9A68C2CE90EA] PRIMARY KEY CLUSTERED 
(
	[product_size_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[room_id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[owner_id] [int] NOT NULL,
	[content] [nvarchar](500) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK__Room__19675A8A174AB013] PRIMARY KEY CLUSTERED 
(
	[room_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK__Service__3E0DB8AFEABC0315] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Size]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[size_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[owner_id] [int] NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK__Size__0DCACE3173588E43] PRIMARY KEY CLUSTERED 
(
	[size_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[staff_id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[password] [char](60) NOT NULL,
	[fullname] [nvarchar](50) NOT NULL,
	[image] [varchar](max) NULL,
	[phone] [varchar](10) NOT NULL,
	[owner_id] [int] NOT NULL,
 CONSTRAINT [PK__Staff__1963DD9C124845EC] PRIMARY KEY CLUSTERED 
(
	[staff_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher](
	[voucher_id] [varchar](8) NOT NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[quantity_used] [int] NOT NULL,
	[start_date] [datetime] NOT NULL,
	[end_date] [datetime] NOT NULL,
	[owner_id] [int] NOT NULL,
	[isdelete] [bit] NOT NULL,
 CONSTRAINT [PK__Voucher__80B6FFA8083E8B23] PRIMARY KEY CLUSTERED 
(
	[voucher_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse](
	[warehouse_id] [int] IDENTITY(1,1) NOT NULL,
	[owner_id] [int] NOT NULL,
	[total_quantity] [int] NOT NULL,
	[total_price] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[warehouse_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WarehouseDetail]    Script Date: 7/16/2024 1:42:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseDetail](
	[warehouse_id] [int] NOT NULL,
	[product_size_id] [nvarchar](30) NOT NULL,
	[quantity_in_stock] [int] NOT NULL,
	[location] [nvarchar](50) NOT NULL,
	[unit_price] [float] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ImportProduct] ADD  CONSTRAINT [DF_ImportProduct_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[ImportProduct] ADD  CONSTRAINT [DF_ImportProduct_total_price]  DEFAULT ((0)) FOR [total_price]
GO
ALTER TABLE [dbo].[ImportProductDetail] ADD  CONSTRAINT [DF_ImportProductDetail_quantity_received]  DEFAULT ((0)) FOR [quantity_received]
GO
ALTER TABLE [dbo].[ImportProductDetail] ADD  CONSTRAINT [DF_ImportProductDetail_unit_price]  DEFAULT ((0)) FOR [unit_price]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_total_amount]  DEFAULT ((0)) FOR [total_amount]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_quantity_sold]  DEFAULT ((0)) FOR [quantity_sold]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_rate_point]  DEFAULT ((0)) FOR [rate_point]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_rate_count]  DEFAULT ((0)) FOR [rate_count]
GO
ALTER TABLE [dbo].[ProductSize] ADD  CONSTRAINT [DF_ProductSize_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Voucher] ADD  CONSTRAINT [DF_Voucher_price]  DEFAULT ((0)) FOR [price]
GO
ALTER TABLE [dbo].[Voucher] ADD  CONSTRAINT [DF_Voucher_quantity]  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Voucher] ADD  CONSTRAINT [DF_Voucher_quantity_used]  DEFAULT ((0)) FOR [quantity_used]
GO
ALTER TABLE [dbo].[Warehouse] ADD  CONSTRAINT [DF_Warehouse_total_quantity]  DEFAULT ((0)) FOR [total_quantity]
GO
ALTER TABLE [dbo].[Warehouse] ADD  CONSTRAINT [DF_Warehouse_total_price]  DEFAULT ((0)) FOR [total_price]
GO
ALTER TABLE [dbo].[WarehouseDetail] ADD  CONSTRAINT [DF_WarehouseDetail_quantity_in_stock]  DEFAULT ((0)) FOR [quantity_in_stock]
GO
ALTER TABLE [dbo].[WarehouseDetail] ADD  CONSTRAINT [DF_WarehouseDetail_unit_price]  DEFAULT ((0)) FOR [unit_price]
GO
ALTER TABLE [dbo].[Advertisement]  WITH CHECK ADD  CONSTRAINT [fk_advertisement_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Advertisement] CHECK CONSTRAINT [fk_advertisement_owner]
GO
ALTER TABLE [dbo].[Advertisement]  WITH CHECK ADD  CONSTRAINT [fk_advertisement_poststatus] FOREIGN KEY([status_post_id])
REFERENCES [dbo].[PostStatus] ([status_post_id])
GO
ALTER TABLE [dbo].[Advertisement] CHECK CONSTRAINT [fk_advertisement_poststatus]
GO
ALTER TABLE [dbo].[Advertisement]  WITH CHECK ADD  CONSTRAINT [fk_advertisement_service] FOREIGN KEY([service_id])
REFERENCES [dbo].[Service] ([service_id])
GO
ALTER TABLE [dbo].[Advertisement] CHECK CONSTRAINT [fk_advertisement_service]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [fk_category_categoryparent] FOREIGN KEY([cate_parent_id])
REFERENCES [dbo].[CategoryParent] ([cate_parent_id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [fk_category_categoryparent]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [fk_comment_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [fk_comment_account]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [fk_comment_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [fk_comment_owner]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [fk_comment_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [fk_comment_product]
GO
ALTER TABLE [dbo].[GuestConsultation]  WITH CHECK ADD  CONSTRAINT [fk_guestconsultation_advertisement] FOREIGN KEY([ad_id])
REFERENCES [dbo].[Advertisement] ([ad_id])
GO
ALTER TABLE [dbo].[GuestConsultation] CHECK CONSTRAINT [fk_guestconsultation_advertisement]
GO
ALTER TABLE [dbo].[GuestConsultation]  WITH CHECK ADD  CONSTRAINT [fk_guestconsultation_gueststatus] FOREIGN KEY([status_guest_id])
REFERENCES [dbo].[GuestStatus] ([status_guest_id])
GO
ALTER TABLE [dbo].[GuestConsultation] CHECK CONSTRAINT [fk_guestconsultation_gueststatus]
GO
ALTER TABLE [dbo].[GuestConsultation]  WITH CHECK ADD  CONSTRAINT [fk_guestconsultation_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[GuestConsultation] CHECK CONSTRAINT [fk_guestconsultation_owner]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [fk_image_description] FOREIGN KEY([description_id])
REFERENCES [dbo].[Description] ([description_id])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [fk_image_description]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [fk_image_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [fk_image_product]
GO
ALTER TABLE [dbo].[ImportProduct]  WITH CHECK ADD  CONSTRAINT [fk_importproduct_warehouse] FOREIGN KEY([warehouse_id])
REFERENCES [dbo].[Warehouse] ([warehouse_id])
GO
ALTER TABLE [dbo].[ImportProduct] CHECK CONSTRAINT [fk_importproduct_warehouse]
GO
ALTER TABLE [dbo].[ImportProductDetail]  WITH CHECK ADD  CONSTRAINT [fk_importproductdetail_importproduct] FOREIGN KEY([import_id])
REFERENCES [dbo].[ImportProduct] ([import_id])
GO
ALTER TABLE [dbo].[ImportProductDetail] CHECK CONSTRAINT [fk_importproductdetail_importproduct]
GO
ALTER TABLE [dbo].[ImportProductDetail]  WITH CHECK ADD  CONSTRAINT [fk_importproductdetail_productsize] FOREIGN KEY([product_size_id])
REFERENCES [dbo].[ProductSize] ([product_size_id])
GO
ALTER TABLE [dbo].[ImportProductDetail] CHECK CONSTRAINT [fk_importproductdetail_productsize]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [fk_message_room] FOREIGN KEY([room_id])
REFERENCES [dbo].[Room] ([room_id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [fk_message_room]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_notification_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_notification_account]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_notification_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_notification_owner]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_order_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_order_account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_order_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_order_owner]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_order_status] FOREIGN KEY([status_id])
REFERENCES [dbo].[OrderStatus] ([status_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_order_status]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_order_voucher] FOREIGN KEY([voucher_id])
REFERENCES [dbo].[Voucher] ([voucher_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_order_voucher]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [fk_orderdetail_order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Order] ([order_id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [fk_orderdetail_order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [fk_orderdetail_productsize] FOREIGN KEY([product_size_id])
REFERENCES [dbo].[ProductSize] ([product_size_id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [fk_orderdetail_productsize]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_brand] FOREIGN KEY([brand_id])
REFERENCES [dbo].[Brand] ([brand_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_brand]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Category] ([category_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_description] FOREIGN KEY([description_id])
REFERENCES [dbo].[Description] ([description_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_description]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_owner]
GO
ALTER TABLE [dbo].[ProductSize]  WITH CHECK ADD  CONSTRAINT [fk_productsize_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[ProductSize] CHECK CONSTRAINT [fk_productsize_product]
GO
ALTER TABLE [dbo].[ProductSize]  WITH CHECK ADD  CONSTRAINT [fk_productsize_size] FOREIGN KEY([size_id])
REFERENCES [dbo].[Size] ([size_id])
GO
ALTER TABLE [dbo].[ProductSize] CHECK CONSTRAINT [fk_productsize_size]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [fk_room_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [fk_room_account]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [fk_room_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [fk_room_owner]
GO
ALTER TABLE [dbo].[Size]  WITH CHECK ADD  CONSTRAINT [fk_size_ower] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Size] CHECK CONSTRAINT [fk_size_ower]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [fk_staff_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [fk_staff_owner]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [fk_voucher_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [fk_voucher_owner]
GO
ALTER TABLE [dbo].[Warehouse]  WITH CHECK ADD  CONSTRAINT [fk_warehouse_owner] FOREIGN KEY([owner_id])
REFERENCES [dbo].[Owner] ([owner_id])
GO
ALTER TABLE [dbo].[Warehouse] CHECK CONSTRAINT [fk_warehouse_owner]
GO
ALTER TABLE [dbo].[WarehouseDetail]  WITH CHECK ADD  CONSTRAINT [fk_warehousedetail_productsize] FOREIGN KEY([product_size_id])
REFERENCES [dbo].[ProductSize] ([product_size_id])
GO
ALTER TABLE [dbo].[WarehouseDetail] CHECK CONSTRAINT [fk_warehousedetail_productsize]
GO
ALTER TABLE [dbo].[WarehouseDetail]  WITH CHECK ADD  CONSTRAINT [fk_warehousedetail_warehouse] FOREIGN KEY([warehouse_id])
REFERENCES [dbo].[Warehouse] ([warehouse_id])
GO
ALTER TABLE [dbo].[WarehouseDetail] CHECK CONSTRAINT [fk_warehousedetail_warehouse]
GO
