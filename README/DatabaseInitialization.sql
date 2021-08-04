
--Создание базы данных
if not exists(select * from sys.databases where name = 'Game_test_Kochurov')
	begin
		create database  Game_test_Kochurov
	end

go

--переключение контекста базы данных
use [Game_test_Kochurov]

go
 
 --Таблица для хранения игроков
if not exists(select * from information_schema.tables where table_name = 'players')
	begin
		create table players
		(		
		player_name varchar(50) unique clustered  not null,
		player_lvl int default 0 not null,
		player_coins int default 1000 not null,
		player_password varchar(30),
		constraint PK_players primary key(player_name) 
		)
	end
go

--таблица для хранения всех пушек
if not exists(select * from information_schema.tables where table_name = 'guns')
	begin
		create table guns
		(		
		gun_name varchar(30) not null unique clustered,
		gun_price int not null,
		gun_damage int not null,
		gun_rate_of_fire int not null,
		gun_recharge_rate int not null,
		gun_num_bullets int not null,
		gun_maxlevel int not null,
		gun_price_step decimal(3,2),
		gun_damage_step decimal(3,2),
		gun_rof_step decimal(3,2),
		gun_rrate_step decimal(3,2),
		gun_num_bullet_step decimal(3,2)
		constraint PK_guns primary key(gun_name) 
		)
	end

go

--таблица для хранения всех персонажей
if not exists(select * from information_schema.tables where table_name = 'characters')
	begin
		create table characters
		(		
		char_name varchar(30) not null unique clustered,
		char_price int
		constraint PK_characters primary key(char_name) 
		)
	end

go

--связь между персонажами и игроками 
--хранятся купленные игроком персонажи
if not exists(select * from information_schema.tables where table_name = 'players_characters')
	begin
		create table players_characters
		(
		player_name varchar(30),
		char_name varchar(30)
		)
	end

go

--связь между персонажами и пшками
--хранятся доступные персонажам пушки
if not exists(select * from information_schema.tables where table_name = 'char_guns')
	begin
		create table char_guns
		(
		char_name varchar(30),
		gun_name varchar(30)
		)
	end

go

--связь между игроками и пушками
--хранятся купленные игроками пушки
if not exists(select * from information_schema.tables where table_name = 'players_guns')
	begin
		create table players_guns
		(
		player_name varchar(30),
		gun_name varchar(30),
		gun_lvl int,
		)
	end

go

--процедура для получения всех пушек и пушек игрока
--параметры пушек игрока расчитываются от прокачанного игроком уровня пушки
create or alter procedure GetGuns @playerName varchar(30) as
begin
select player_name, 
       players_guns.gun_name, 
	   convert(int,(gun_price + gun_price * (gun_lvl * gun_price_step))) as gun_price,
	   gun_lvl, 
	   convert(int,(gun_damage + gun_damage * (gun_lvl * gun_damage_step))) as gun_damage,
	   convert(int,(gun_rate_of_fire + gun_rate_of_fire *(gun_lvl * gun_rof_step))) as gun_rate_of_fire,
	   convert(int,(gun_recharge_rate + gun_recharge_rate * (gun_lvl * gun_rrate_step))) as gun_recharge_rate,
	   convert(int,(gun_num_bullets + gun_num_bullets*(gun_lvl * gun_num_bullet_step))) as gun_num_bullets,
	   case when player_name = @playerName then 1 else 0 end as gun_unlocked
from players_guns
left join guns on guns.gun_name = players_guns.gun_name
where player_name = @playerName
union all
select null as player_name, gun_name, gun_price, 0, gun_damage, gun_rate_of_fire, gun_recharge_rate, gun_num_bullets,0 
from guns 
where gun_name not in (select gun_name from players_guns where player_name = @playerName) 
end

go

--процедура для получения доступных персонажу пушек
create or alter procedure GetGunsCharacter @playerName varchar(30), @characterName varchar(30) as
begin
select player_name, 
       players_guns.gun_name, 
	   convert(int,(gun_price + gun_price * (gun_lvl * gun_price_step))) as gun_price,
	   gun_lvl, 
	   convert(int,(gun_damage + gun_damage * (gun_lvl * gun_damage_step))) as gun_damage,
	   convert(int,(gun_rate_of_fire + gun_rate_of_fire *(gun_lvl * gun_rof_step))) as gun_rate_of_fire,
	   convert(int,(gun_recharge_rate + gun_recharge_rate * (gun_lvl * gun_rrate_step))) as gun_recharge_rate,
	   convert(int,(gun_num_bullets + gun_num_bullets*(gun_lvl * gun_num_bullet_step))) as gun_num_bullets,
	   case when player_name = @playerName then 1 else 0 end as gun_unlocked
from players_guns
left join guns on guns.gun_name = players_guns.gun_name
where player_name = @playerName and guns.gun_name in (select gun_name from char_guns where char_guns.char_name = @characterName)
union all
select null as player_name, gun_name, gun_price, 0, gun_damage, gun_rate_of_fire, gun_recharge_rate, gun_num_bullets,0 
from guns 
where gun_name not in (select gun_name from players_guns where player_name = @playerName) 
and  guns.gun_name in (select gun_name from char_guns where char_guns.char_name = @characterName)
end

go

--процедура для реализации покупки пушек и персонажей игроком
create or alter procedure BuyItem @playerName varchar(30), @itemName varchar(30) as
begin
	if (exists(select @itemName from guns)) 
		begin
			declare @coins int = (select player_coins from players where player_name = @playerName) -  (select gun_price from guns where gun_name = @itemName)
			update players set player_coins = @coins where player_name = @playerName;
			insert into players_guns values (@playerName, @itemName, 1);
		end
	else 
	if (exists(select @itemName from characters))
		begin
			set @coins = (select player_coins from players where player_name = @playerName) -  (select char_price from characters where char_name = @itemName)
			update players set player_coins = @coins where player_name = @playerName;
			insert into players_characters values (@playerName, @itemName);	
		end
end

go

--процедура для прокачки игроком своей пушки
create or alter procedure UpItem @playerName varchar(30), @itemName varchar(30) as
begin
	if exists(select * from players_guns where player_name = @playerName and gun_name = @itemName)
	begin
		declare @coin int 
		declare @gunPlayerTable table (player_name varchar(30),
									   gun_name varchar(30),
									   gun_price int,
									   gun_lvl int,
									   gun_damage int,
									   gun_rate_of_fire int,
									   gun_recharge_rate int,
									   gun_num_bullets int,
									   gun_unlocked int
									  )
		insert into @gunPlayerTable exec GetGuns @playerName 

		set @coin = (select player_coins from players where player_name = @playerName) - (select gun_price from @gunPlayerTable where player_name = @playerName and gun_name = @itemName )

		update players set player_coins = @coin where player_name = @playerName;
		update players_guns set gun_lvl = gun_lvl + 1 where player_name = @playerName and gun_name = @itemName;

	end
end

go

--вставка тестовых пользователей
INSERT INTO [dbo].[players] ([player_name],[player_lvl],[player_coins],[player_password])
     VALUES ('Player1',1,1000,'Passw1'),
	        ('Player2',1,1000,'Passw2'),
			('Player3',1,1000,'Passw3'),
			('Player4',1,1000,'Passw4'),
			('Player5',1,1000,'Passw5')

go

--вставка тестовых пушек
INSERT INTO [dbo].[guns]
           ([gun_name]
           ,[gun_price]
           ,[gun_damage]
           ,[gun_rate_of_fire]
           ,[gun_recharge_rate]
           ,[gun_num_bullets]
           ,[gun_maxlevel]
           ,[gun_price_step]
           ,[gun_damage_step]
           ,[gun_rof_step]
           ,[gun_rrate_step]
           ,[gun_num_bullet_step])
     VALUES
           ('gun0', 100, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun1', 100, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun2', 110, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun3', 110, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun4', 110, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun5', 100, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun6', 130, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun7', 100, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun8', 150, 5, 3, 2, 5, 10, 0.2, 0.2 ,0.2 ,0.2 ,0.2),
		   ('gun9', 200, 20, 5, 5, 15, 10, 0.35, 0.35 ,0.24 ,0.3 ,0.5)

go

--вставка тестовых персонажей
INSERT INTO [dbo].[characters] ([char_name] ,[char_price])
     VALUES
           ('Char1' ,100),
		   ('Char2' ,100),
		   ('Char3' ,120),
		   ('Char4' ,130),
		   ('Char5' ,150)

go

--назначение тестовым персонажам тестовых пушек
INSERT INTO [dbo].[char_guns] ([char_name] ,[gun_name])
     VALUES
           ('Char1' ,'gun0'),
		   ('Char1' ,'gun1'),
		   ('Char2' ,'gun2'),
		   ('Char2' ,'gun3'),
		   ('Char3' ,'gun4'),
		   ('Char3' ,'gun5'),
		   ('Char4' ,'gun6'),
		   ('Char4' ,'gun7'),
		   ('Char5' ,'gun8'),
		   ('Char5' ,'gun9')


go

--выдача тестовым игрокам тестовых персонажей
INSERT INTO [dbo].[players_characters] ([player_name] ,[char_name])
     VALUES
           ('Player1','Char1'),
		   ('Player2','Char1'),
		   ('Player3','Char1'),
		   ('Player4','Char1'),
		   ('Player5','Char1')

go

--выдача тестовым игрокам тестовых пушек
INSERT INTO [dbo].[players_guns] ([player_name] ,[gun_name] ,[gun_lvl])
     VALUES
           ('Player1' ,'gun1' ,1),
		   ('Player2' ,'gun1' ,1),
		   ('Player3' ,'gun1' ,1),
		   ('Player4' ,'gun1' ,1),
		   ('Player5' ,'gun1' ,1)