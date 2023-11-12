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

create table Negocio(
IdNegocio int primary key,
Nombre varchar(50),
RUC varchar(50),
Direccion varchar(100),
Logo varbinary(max) null 
)


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
/////////////////////////////////////////////////////////////////////////////////////

create proc sp_EliminarUsuario(
@IdUsuario int,
@Resultado bit output,
@Mensaje varchar(100) output
)
as

begin
		set @Resultado=0
		set @Mensaje=''
		declare @PasoReglas bit =1

		IF exists(select * from Compra where IdUsuario =@IdUsuario)
		begin
			set @PasoReglas=0
			set @Resultado= 0
			set @Mensaje='Este Usuario esta relacionado con alguna Compra.\n'
		end
		IF exists(select * from Venta where IdUsuario =@IdUsuario)
		begin
			set @PasoReglas=0
			set @Resultado= 0
			set @Mensaje='Este Usuario esta relacionado con alguna Venta.\n'
		end

		if(@PasoReglas=1)
		begin
			
			delete from Usuario where IdUsuario=@IdUsuario
			set @Resultado=1
		end
end

////////////////////////////////////////////////////////////////////////////////

create proc sp_AgregarCategoria(
@Descripcion varchar(50),
@Estado bit,
@IdGeneradoResultado int output,
@Mensaje varchar(100) output
)
as
begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Categoria where Descripcion=@Descripcion)
		begin
			insert into Categoria(Descripcion,Estado) values(@Descripcion,@Estado)

			set @IdGeneradoResultado= SCOPE_IDENTITY()
		end
		else
		begin
			set @Mensaje='No se puede agregar una categoria con la misma descripcion'
		end
end
///////////////////////////////////////////////////////////////////////////////////

create proc sp_ModificarCategoria(
@IdCategoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(100) output
)
as
begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Categoria where Descripcion=@Descripcion and IdCategoria!=@IdCategoria)
		begin
			update Categoria set Descripcion=@Descripcion,Estado=@Estado where IdCategoria=@IdCategoria

			set @Resultado= 1
		end
		else
		begin
			set @Mensaje='ya existe una categoria con la misma descripcion'
		end
end
///////////////////////////////////////////////////////////////////////////////////////////////

create proc sp_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje varchar(100) output
)
as
begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Producto where IdCategoria=@IdCategoria)
		begin
			delete from Categoria where IdCategoria=@IdCategoria
				set @Resultado=1
			
		end
		else
			begin
				set @Mensaje='No se puede eliminar por que la categoria esta relacionada a un producto'	
			end
end
/////////////////////////////////////////////////////////////////////////////////////////////////


create proc sp_AgregarProducto(
@Codigo varchar(20),
@Nombre varchar(50),
@Descripcion varchar(100),
@IdCategoria int, 
@Estado bit,
@IdGeneradoResultado int output,
@Mensaje varchar(100) output
)
as
begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Producto where Codigo=@Codigo )
		begin
			insert into Producto(Codigo,Nombre,Descripcion,IdCategoria,Estado)
			values(@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)

			set @IdGeneradoResultado=SCOPE_IDENTITY()
		end
		else
		begin
			set @Mensaje='Ya existe un Producto con el mismo codigo'
		end
end
//////////////////////////////////////////////////////////////////////////////////////////////////

create proc sp_ModificarProducto(
@IdProducto int,
@Codigo varchar(20),
@Nombre varchar(50),
@Descripcion varchar(100),
@IdCategoria int, 
@Estado bit,
@IdGeneradoResultado bit output,
@Mensaje varchar(100) output
)
as
begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Producto where Codigo=@Codigo and IdProducto!=@IdProducto )
		begin
			update Producto set Codigo=@Codigo,Nombre=@Nombre,Descripcion=@Descripcion,
			IdCategoria=@IdCategoria,Estado=@Estado where IdProducto=@IdProducto
			
			set @IdGeneradoResultado=1
		end
		else
		begin
			set @Mensaje='No se puede agregar un producto con el mismo codigo'
		end
end
///////////////////////////////////////////////////////////////////////////////////////////////////////
create proc sp_EliminarProducto(
@IdProducto int,
@Resultado bit output,
@Mensaje varchar(100) output
)
as
begin	
		set @Resultado=0
		set @Mensaje=''
		declare @cumpleReglas bit=1

		if exists(select * from DetalleCompra where IdProducto=@IdProducto)
		begin
			set @Resultado=0
			set @Mensaje='No se puede eliminar por que el producto esta relacionado con alguna compra'
			set @cumpleReglas =0
				
		end
		if exists(select * from DetalleVenta where IdProducto=@IdProducto)
		begin
			set @Resultado=0
			set @Mensaje='No se puede eliminar por que el producto esta relacionado con alguna Venta'
			set @cumpleReglas =0
				
		end

		if(@cumpleReglas=1)
			begin
				delete from Producto where IdProducto=@IdProducto
				set @Resultado=1
			end
end
/////////////////////////////////////////////////////////////////////////////////////////////////////


create proc sp_AgregarCliente(

@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Telefono varchar(20),
@Estado bit ,
@IdGeneradoResultado int output,
@Mensaje varchar output
)
as
begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Cliente where Documento=@Documento)
		begin
			insert into Cliente(Documento,NombreCompleto,Correo,Telefono,Estado)
			values(@Documento,@NombreCompleto,@Correo,@Telefono,@Estado)

			set @IdGeneradoResultado= SCOPE_IDENTITY()
		end
		else
		begin
			set @Mensaje='No se puede registrar un cliente con el mismo Documento'
		end
end
///////////////////////////////////////////////////////////////////////////////////////////////

create proc sp_ModificarCliente(
@IdCliente int,
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Telefono varchar(20),
@Estado bit ,
@Resultado bit output,
@Mensaje varchar output
)
as
begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Cliente where Documento=@Documento and IdCliente!=@IdCliente)
		begin
			update Cliente set Documento=@Documento,
			NombreCompleto=@NombreCompleto,Correo=@Correo,Telefono=@Telefono,Estado=@Estado
			where IdCliente=@IdCliente

			set @Resultado= 1
		end
		else
		begin
			set @Mensaje='Ya existe un cliente con el mismo Documento '
		end
end
///////////////////////////////////////////////////////////////////////////////////

create proc sp_AgregarProveedor(

@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(100),
@Telefono varchar(20),
@Estado bit ,
@IdGeneradoResultado int output,
@Mensaje varchar output
)
as
begin
		set @IdGeneradoResultado=0
		set @Mensaje=''

		if not exists(select * from Proveedor where Documento=@Documento)
		begin
			insert into Proveedor(Documento,RazonSocial,Correo,Telefono,Estado)
			values(@Documento,@RazonSocial,@Correo,@Telefono,@Estado)

			set @IdGeneradoResultado= SCOPE_IDENTITY()
		end
		else
		begin
			set @Mensaje='No se puede registrar un Proveedor con el mismo Documento'
		end
end

/////////////////////////////////////////////////////////////////////////////////////////

create proc sp_ModificarProveedor(
@IdProveedor int,
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(100),
@Telefono varchar(20),
@Estado bit ,
@Resultado bit output,
@Mensaje varchar output
)
as
begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Proveedor where Documento=@Documento and IdProveedor!=@IdProveedor)
		begin
			update Proveedor set Documento=@Documento,
			RazonSocial=@RazonSocial,Correo=@Correo,Telefono=@Telefono,Estado=@Estado
			where IdProveedor=@IdProveedor

			set @Resultado= 1
		end
		else
		begin
			set @Mensaje='Ya existe un Proveedor con el mismo Documento '
		end
end
///////////////////////////////////////////////////////////////////////////////////////////////
create proc sp_EliminarProveedor(
@IdProveedor int,
@Resultado bit output,
@Mensaje varchar(100) output
)
as
begin
		set @Resultado=0
		set @Mensaje=''

		if not exists(select * from Compra where IdProveedor=@IdProveedor)
		begin
			delete from Proveedor where IdProveedor=@IdProveedor
				set @Resultado=1
			
		end
		else
			begin
				set @Mensaje='No se puede eliminar por que el proveedor esta relacionada a una compra'	
			end
end

////////////////////////////////////////////////////////////////////////////////////////////////////