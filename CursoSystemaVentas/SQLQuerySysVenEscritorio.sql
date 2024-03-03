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

create type[dbo].[EDetalle_Compra] as Table
(
[IdProducto] int ,
[PrecioCompra] decimal(10,2) null,
[PrecioVenta] decimal(10,2) null,
[Cantidad] int null,
[Total] decimal(10,2) null
)
go

create procedure sp_RegistrarCompra
(
@IdUsuario int ,
@IdProveedor int ,
@TipoDocumento varchar(100),
@NumeroDocumento varchar(20),
@MontoTotal decimal(10,2),
@EDetalle_Compra [EDetalle_Compra] readonly,
@Resultado bit output,
@Mensaje varchar(500) output 
)
as
begin
	
	begin try
			declare @IdCompra int= 0
			set @Resultado=1
			set @Mensaje=''

			begin transaction Registro
					
					insert into Compra(IdUsuario,IdProveedor,TipoDocumento,NumeroDocumento,MontoTotal)
							values(@IdUsuario,@IdProveedor,@TipoDocumento,@NumeroDocumento,@MontoTotal)

					set @IdCompra= SCOPE_IDENTITY()

					insert into DetalleCompra(IdCompra, IdProducto, PrecioCompra,PrecioVenta,Cantidad,Total)
							select @IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,Total from @EDetalle_Compra

					update p set p.Stock= p.Stock + dc.Cantidad,
					p.PrecioCompra= dc.PrecioCompra,
					p.PrecioVenta= dc.PrecioVenta
					from Producto p
					join @EDetalle_Compra dc
					on dc.IdProducto = p.IdProducto


			commit transaction Registro

	end try

	begin catch
			set @Resultado=0
			set @Mensaje=ERROR_MESSAGE()
			rollback transaction Registro
	end catch

end

//////////////////////////////////////////////////////////////////////////////
/*para detalle de Venta*/
select c.IdCompra,
u.NombreCompleto,
p.Documento,p.RazonSocial,
c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,CONVERT(char(10), c.FechaCreacion,103)[FechaRegistro]
from Compra c
join Proveedor p on p.IdProveedor=c.IdProveedor
join Usuario u on u.IdUsuario=c.IdUsuario
where c.NumeroDocumento='00001'


select p.Nombre,
dc.PrecioCompra,dc.Cantidad,dc.Total
 from DetalleCompra dc
join Compra c on c.IdCompra=dc.IdCompra
join Producto p on p.IdProducto=dc.IdProducto
where dc.IdCompra=1
go
////////////////////////////////////////////////////////////////////////////////

create type[dbo].[EDetalle_Venta] as Table
(
[IdProducto] int ,
[PrecioVenta] decimal(10,2) null,
[Cantidad] int null,
[SubTotal] decimal(10,2) null
)
go

create proc sp_RegistrarVenta(
@IdUsuario int ,
@TipoDocumento varchar(100),
@NumeroDocumento varchar(20),
@DocumentoCliente varchar(20),
@NombreCliente varchar(100),
@MontoPago decimal(10,2),
@MontoCambio decimal(10,2),
@MontoTotal decimal(10,2),
@EDetalle_Venta [EDetalle_Venta] readonly,
@Resultado bit output,
@Mensaje varchar(500) output 
)
as
begin
	
	begin try
			declare @IdVenta int= 0
			set @Resultado=1
			set @Mensaje=''

			begin transaction Registro
					
					insert into Venta(IdUsuario,TipoDocumento,NumeroDocumento,DocumentoCliente,NombreCliente,
								MontoPago,MontoCambio, MontoTotal)
							values(@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,
								@MontoPago,@MontoCambio, @MontoTotal)

					set @IdVenta= SCOPE_IDENTITY()

					insert into DetalleVenta(IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal)
							select @IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal from @EDetalle_Venta

					update p set p.Stock= p.Stock - dv.Cantidad
					from Producto p
					join @EDetalle_Venta dv
					on dv.IdProducto = p.IdProducto


			commit transaction Registro

	end try

	begin catch
			set @Resultado=0
			set @Mensaje=ERROR_MESSAGE()
			rollback transaction Registro
	end catch

end
///////////////////////////////////////////////////////////////////////////////////////////////////
/*para detalle de Venta*/
select v.IdVenta,
u.NombreCompleto,
v.DocumentoCliente,v.NombreCliente,
v.TipoDocumento, v.NumeroDocumento,v.MontoPago,v.MontoCambio,v.MontoTotal,CONVERT(char(10), v.FechaCreacion,103)[FechaRegistro]
from Venta v
join Usuario u on u.IdUsuario=v.IdUsuario
where v.NumeroDocumento='00001' 

select p.Nombre,
dv.PrecioVenta,dv.Cantidad,dv.SubTotal
from DetalleVenta dv
join Venta v on v.IdVenta=dv.IdVenta
join Producto p on p.IdProducto=dv.IdProducto
where dv.IdVenta=1

///////////////////////////////////////////////////////////////////////////////////////////////////////
go

select 
CONVERT(char(10),c.FechaCreacion,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
u.NombreCompleto[UsuarioRegistro],
pr.Documento[DocumentoProveedor],pr.RazonSocial,
p.Codigo[CodigoProducto],p.Nombre[NombreProducto],
ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.Total[SubTotal]
from Compra c
join Usuario u on u.IdUsuario=c.IdUsuario
join Proveedor pr on pr.IdProveedor=c.IdProveedor
join DetalleCompra dc on dc.IdCompra=c.IdCompra
join Producto p on p.IdProducto= dc.IdProducto
join Categoria ca on ca.IdCategoria=p.IdCategoria
where CONVERT(date,c.FechaCreacion) between '' and ''
and pr.IdProveedor=1
/////////////////////////////////////////////////////////////////////////////////////////////////////
go

create Proc sp_ReporteCompra(
@FechaInicio varchar(10),
@FechaFin varchar(10),
@IdProveedor int
)
as
begin

		set DateFormat dmy;

		select 
		CONVERT(char(10),c.FechaCreacion,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
		u.NombreCompleto[UsuarioRegistro],
		pr.Documento[DocumentoProveedor],pr.RazonSocial,
		p.Codigo[CodigoProducto],p.Nombre[NombreProducto],
		ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.Total[SubTotal]
		from Compra c
		join Usuario u on u.IdUsuario=c.IdUsuario
		join Proveedor pr on pr.IdProveedor=c.IdProveedor
		join DetalleCompra dc on dc.IdCompra=c.IdCompra
		join Producto p on p.IdProducto= dc.IdProducto
		join Categoria ca on ca.IdCategoria=p.IdCategoria
		where CONVERT(date,c.FechaCreacion) between @FechaInicio and @FechaFin
		and pr.IdProveedor=iif(@IdProveedor=0, pr.IdProveedor, @IdProveedor)

end
/////////////////////////////////////////////////////////////////////////////////////////////////////////


create proc sp_ReporteVenta(
@FechaInicio varchar(10),
@FechaFin varchar(10)
)
as
begin
		set DateFormat dmy;

		select 
		CONVERT(char(10),v.FechaCreacion,103)[FechaRegistro],v.TipoDocumento,v.NumeroDocumento,v.MontoTotal,
		u.NombreCompleto[UsuarioRegistro],
		v.DocumentoCliente,v.NombreCliente,
		p.Codigo[CodigoProducto],p.Nombre[NombreProducto],
		c.Descripcion[Categoria],
		dv.PrecioVenta,dv.Cantidad,dv.SubTotal

		from Venta v
		join Usuario u on u.IdUsuario=v.IdUsuario
		join DetalleVenta dv on dv.IdVenta=v.IdVenta
		join Producto p on p.IdProducto=dv.IdProducto
		join Categoria c on c.IdCategoria= p.IdCategoria
		where CONVERT(date,v.FechaCreacion) between @FechaInicio and @FechaFin
end
	

	