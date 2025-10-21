drop database cards_products;

create database cards_products;

-- Crear un usuario para esta base
create user 'paradigmas'@'localhost' identified by '123456';

-- Dar permisos al usuario en esta base
grant all privileges on cards_products.* to 'paradigmas'@'localhost';
flush privileges;

use cards_products;

select * from products;

select * from cards;

select * from purchases;