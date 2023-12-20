CREATE TABLE customers (
  id INT IDENTITY PRIMARY KEY,
  name VARCHAR(100),
  email VARCHAR(255)
);

CREATE TABLE orders (
  id INT IDENTITY PRIMARY KEY,
  customer_id INT FOREIGN KEY REFERENCES customers(id),
  product_id INT,
  quantity INT
);

INSERT INTO customers (name, email) VALUES
  ('John Doe', 'johndoe@example.com'),
  ('Jane Doe', 'janedoe@example.com');

INSERT INTO orders (customer_id, product_id, quantity) VALUES
  (1, 1, 1),
  (1, 2, 2),
  (2, 3, 3);