USE [master]
GO
/****** Object:  Database [SmartParkingDatabase]    Script Date: 6/6/2023 4:51:37 PM ******/
CREATE DATABASE [SmartParkingDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SmartParkingDatabase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SmartParkingDatabase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SmartParkingDatabase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SmartParkingDatabase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SmartParkingDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SmartParkingDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SmartParkingDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SmartParkingDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SmartParkingDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SmartParkingDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SmartParkingDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET RECOVERY FULL 
GO
ALTER DATABASE [SmartParkingDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [SmartParkingDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SmartParkingDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SmartParkingDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SmartParkingDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SmartParkingDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SmartParkingDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SmartParkingDatabase', N'ON'
GO
ALTER DATABASE [SmartParkingDatabase] SET QUERY_STORE = OFF
GO
USE [SmartParkingDatabase]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] NOT NULL,
	[EmployeeName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[ContactNo] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeVehicle]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeVehicle](
	[VehicleId] [int] IDENTITY(1,1) NOT NULL,
	[VehicleType] [varchar](50) NULL,
	[VehicleModel] [varchar](50) NULL,
	[VehicleNumber] [varchar](50) NULL,
	[EmployeeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[VehicleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewEmployeeDetails]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewEmployeeDetails] AS (

  SELECT e.EmployeeId,e.EmployeeName,e.Email,e.ContactNo,ev.VehicleId,ev.VehicleNumber,ev.VehicleType,ev.VehicleModel
  FROM Employee as e
  LEFT JOIN 
  EmployeeVehicle as ev
  on
    e.EmployeeId=ev.EmployeeId 
);
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminId] [int] NOT NULL,
	[AdminName] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[ContactNo] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminToken]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminToken](
	[AdminId] [int] NULL,
	[JWTToken] [varchar](350) NULL,
	[RefreshToken] [varchar](50) NULL,
	[Created] [varchar](20) NULL,
	[Expire] [varchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operator]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operator](
	[OperatorId] [int] NOT NULL,
	[OperatorName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[ContactNo] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[OperatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 6/6/2023 4:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[EmployeeId] [int] NULL,
	[JWTToken] [varchar](350) NULL,
	[RefreshToken] [varchar](50) NULL,
	[Created] [varchar](20) NULL,
	[Expire] [varchar](20) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AdminToken]  WITH CHECK ADD FOREIGN KEY([AdminId])
REFERENCES [dbo].[Admin] ([AdminId])
GO
ALTER TABLE [dbo].[EmployeeVehicle]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[RefreshToken]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
USE [master]
GO
ALTER DATABASE [SmartParkingDatabase] SET  READ_WRITE 
GO
