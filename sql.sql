USE [QLKTX]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Address] [nvarchar](200) NULL,
	[FullName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BedOfRoom]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BedOfRoom](
	[BedID] [int] IDENTITY(1,1) NOT NULL,
	[RoomID] [int] NULL,
	[NumberBed] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[BedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DH]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DH](
	[DHID] [int] IDENTITY(1,1) NOT NULL,
	[DHCode] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DHID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[NewsID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Content] [nvarchar](max) NULL,
	[PublishedDate] [date] NULL,
	[Author] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[NewsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Occupancy]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Occupancy](
	[OccupancyID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NULL,
	[RoomID] [int] NULL,
	[RenewalDate] [date] NULL,
	[ExpirationDate] [date] NULL,
	[CycleMonths] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[OccupancyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[MOWRoom] [nvarchar](10) NULL,
	[Building] [nvarchar](10) NULL,
	[Floor] [int] NULL,
	[NumberRoom] [int] NULL,
	[BedNumber] [int] NULL,
	[NumberOfStudents] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](100) NULL,
	[Month] [int] NULL,
	[Price] [decimal](10, 2) NULL,
	[RoomID] [int] NULL,
	[StudentID] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NULL,
	[DateOfBirth] [date] NULL,
	[Gender] [nvarchar](10) NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[ParentPhoneNumber] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[StudentCode] [nvarchar](20) NULL,
	[DHID] [int] NULL,
	[DepartmentID] [int] NULL,
	[Class] [nvarchar](50) NULL,
	[AdmissionConfirmation] [nvarchar](100) NULL,
	[RoomID] [int] NULL,
	[BedID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 12/31/2024 10:46:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[Description] [nvarchar](200) NULL,
	[TransactionCode] [nvarchar](20) NULL,
	[TransactionDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BedOfRoom]  WITH CHECK ADD  CONSTRAINT [FK_RoomID_Bed] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO
ALTER TABLE [dbo].[BedOfRoom] CHECK CONSTRAINT [FK_RoomID_Bed]
GO
ALTER TABLE [dbo].[Occupancy]  WITH CHECK ADD  CONSTRAINT [FK_RoomID_Occupancy] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO
ALTER TABLE [dbo].[Occupancy] CHECK CONSTRAINT [FK_RoomID_Occupancy]
GO
ALTER TABLE [dbo].[Occupancy]  WITH CHECK ADD  CONSTRAINT [FK_StudentID_Occupancy] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Occupancy] CHECK CONSTRAINT [FK_StudentID_Occupancy]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_RoomID_Service] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_RoomID_Service]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_StudentID_Service] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_StudentID_Service]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_BedID_Student] FOREIGN KEY([BedID])
REFERENCES [dbo].[BedOfRoom] ([BedID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_BedID_Student]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([DepartmentID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_DepartmentID]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_DHID] FOREIGN KEY([DHID])
REFERENCES [dbo].[DH] ([DHID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_DHID]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_RoomID_Student] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_RoomID_Student]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_StudentID_Transaction] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_StudentID_Transaction]
GO
ALTER TABLE [dbo].[BedOfRoom]  WITH CHECK ADD  CONSTRAINT [CHK_BedStatus] CHECK  (([Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[BedOfRoom] CHECK CONSTRAINT [CHK_BedStatus]
GO
ALTER TABLE [dbo].[Occupancy]  WITH CHECK ADD  CONSTRAINT [CHK_OccupancyStatus] CHECK  (([Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[Occupancy] CHECK CONSTRAINT [CHK_OccupancyStatus]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [CHK_RoomStatus] CHECK  (([Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [CHK_RoomStatus]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [CHK_ServiceStatus] CHECK  (([Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [CHK_ServiceStatus]
GO
