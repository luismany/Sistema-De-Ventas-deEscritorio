create Database DB_SysVenEscritorio
go
use DB_SysVenEscritorio
go
create table Rol(
IdRol int primary key identity,
Descripcion varchar(50),
FechaCreacion datetime default getdate() 
)
go

create table Permiso(
IdPermiso int primary key identity,
IdRol int references Rol(IdRol),
NombreMenu varchar(50),
FechaCreacion datetime default getdate() 
)
go

create table Proveedor(
IdProveedor int primary key identity,
Documento varchar(50),
RazonSocial varchar(50),
Correo varchar(100),
Telefono varchar(20),
Estado bit ,
FechaCreacion datetime default getdate() 
)
go
create table Cliente(
IdCliente int primary key identity,
Documento varchar(50),
NombreCompleto varchar(100),
Correo varchar(100),
Telefono varchar(20),
Estado bit ,
FechaCreacion datetime default getdate() 
)
go

create table Usuario(
IdUsuario int primary key identity,
Documento varchar(50),
NombreCompleto varchar(100),
Correo varchar(100),
Clave varchar(20),
IdRol int references Rol(IdRol),
Estado bit ,
FechaCreacion datetime default getdate() 
)
go
create table Categoria(
IdCategoria int primary key identity,
Descripcion varchar(50),
Estado bit,
FechaCreacion datetime default getdate() 
)
go
create table Producto(
IdProducto int primary key identity,
Codigo varchar(20),
Nombre varchar(50),
Descripcion varchar(100),
IdCategoria int references Categoria(IdCategoria),
Stock int not null default 0,
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Estado bit,
FechaCreacion datetime default getdate() 
)
go


create table Compra(
IdCompra int primary key identity,
IdUsuario int references Usuario(IdUsuario),
IdProveedor int references Proveedor(IdProveedor),
TipoDocumento varchar(100),
NumeroDocumento varchar(20),
MontoTotal decimal(10,2),
FechaCreacion datetime default getdate() 
)
go

create table DetalleCompra(
IdDetalleCompra int primary key identity,
IdCompra int references Compra(IdCompra),
IdProducto int references Producto(IdProducto),
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Cantidad int,
Total decimal(10,2), 
FechaCreacion datetime default getdate() 
)
go

create table Venta(
IdVenta int primary key identity,
IdUsuario int references Usuario(IdUsuario),
TipoDocumento varchar(100),
NumeroDocumento varchar(20),
DocumentoCliente varchar(20),
NombreCliente varchar(100),
MontoPago decimal(10,2),
MontoCambio decimal(10,2),
MontoTotal decimal(10,2),
FechaCreacion datetime default getdate() 
)
go

create table DetalleVenta(
IdDetalleVenta int primary key identity,
IdVenta int references Venta(IdVenta),
IdProducto int references Producto(IdProducto),
PrecioVenta decimal(10,2) ,
Cantidad int,
SubTotal decimal(10,2), 
FechaCreacion datetime default getdate() 
)
go


create proc sp_AgregarUsuario(
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(20),
@IdRol int ,
@Estado bit ,
@IdGeneradoResultado int output,
@Mensaje varchar(100) output
)
as

begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Usuario where Documento=@Documento )
		begin
			insert into Usuario(Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
			values(@Documento,@NombreCompleto,@Correo,@Clave,@IdRol,@Estado)

			set @IdGeneradoResultado= SCOPE_IDENTITY()
		end
		else
		begin
			set @Mensaje='Ya existe un Usuario con  el mismo numero de Documento'
		end
end

/////////////////////////////////////////////////////////////////////////////////////

create proc sp_ModificarUsuario(
@IdUsuario int,
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(20),
@IdRol int ,
@Estado bit ,
@Resultado bit output,
@Mensaje varchar(100) output
)
as

begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Usuario where Documento=@Documento and IdUsuario != @IdUsuario )
		begin
			update Usuario set Documento=@Documento,NombreCompleto=@NombreCompleto,
					Correo=@Correo,Clave=@Clave,IdRol=@IdRol,Estado=@Estado where IdUsuario=@IdUsuario
			
			set @Resultado= 1
		end
		else
		begin
			set @Mensaje='Ya existe un Usuario con  el mismo numero de Documento'
		end
end


