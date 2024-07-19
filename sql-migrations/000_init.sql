create table if not exists  sales_managers (
    id serial primary key not null,
    name varchar(250) not null,
    languages varchar(100)[],
    products varchar(100)[],
    customer_ratings varchar(100)[]
);