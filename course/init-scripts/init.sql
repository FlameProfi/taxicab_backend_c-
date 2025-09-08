-- Create tables for taxi application

-- Cars table
CREATE TABLE cars (
    id SERIAL PRIMARY KEY,
    make VARCHAR(100) NOT NULL,
    model VARCHAR(100) NOT NULL,
    year INTEGER,
    price DECIMAL(10,2),
    color VARCHAR(50),
    image_url VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Employees table
CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    position VARCHAR(100),
    department VARCHAR(100),
    email VARCHAR(255) UNIQUE,
    phone VARCHAR(20),
    salary DECIMAL(10,2),
    hire_date DATE,
    image_url VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Taxi calls table
CREATE TABLE taxi_calls (
    id SERIAL PRIMARY KEY,
    client_name VARCHAR(200) NOT NULL,
    client_phone VARCHAR(20),
    call_time TIMESTAMP WITH TIME ZONE,
    pickup_address VARCHAR(500),
    destination_address VARCHAR(500),
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'accepted', 'completed', 'cancelled', 'no_answer')),
    driver_name VARCHAR(200),
    car_model VARCHAR(100),
    car_number VARCHAR(20),
    price DECIMAL(10,2),
    duration INTEGER,
    distance DECIMAL(8,2),
    rating INTEGER CHECK (rating >= 1 AND rating <= 5),
    notes VARCHAR(1000),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Driver applications table
CREATE TABLE driver_applications (
    id SERIAL PRIMARY KEY,
    full_name VARCHAR(200) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(255),
    driving_experience INTEGER,
    car_model VARCHAR(100),
    car_year INTEGER,
    message VARCHAR(1000),
    status VARCHAR(50) DEFAULT 'pending' CHECK (status IN ('pending', 'approved', 'rejected')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Users table (new)
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role VARCHAR(20) DEFAULT 'Dispatcher',
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    phone VARCHAR(20),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes
CREATE INDEX idx_cars_make ON cars(make);
CREATE INDEX idx_cars_model ON cars(model);
CREATE INDEX idx_employees_email ON employees(email);
CREATE INDEX idx_employees_department ON employees(department);
CREATE INDEX idx_taxi_calls_time ON taxi_calls(call_time);
CREATE INDEX idx_taxi_calls_status ON taxi_calls(status);
CREATE INDEX idx_taxi_calls_client ON taxi_calls(client_name);
CREATE INDEX idx_driver_applications_phone ON driver_applications(phone);
CREATE INDEX idx_driver_applications_status ON driver_applications(status);
CREATE INDEX idx_driver_applications_created ON driver_applications(created_at);
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);

-- Insert test data
INSERT INTO cars (make, model, year, price, color, image_url) VALUES
('Toyota', 'Camry', 2022, 25000.00, 'White', 'https://example.com/toyota.jpg'),
('Honda', 'Civic', 2021, 22000.00, 'Blue', 'https://example.com/honda.jpg'),
('Ford', 'Mustang', 2023, 35000.00, 'Red', 'https://example.com/ford.jpg');

INSERT INTO employees (first_name, last_name, position, department, email, phone, salary, hire_date) VALUES
('Ivan', 'Petrov', 'Sales Manager', 'Sales Department', 'ivan.petrov@company.com', '+79991234567', 65000.00, '2022-03-15'),
('Maria', 'Sidorova', 'Financial Analyst', 'Finance Department', 'maria.sidorova@company.com', '+79992345678', 75000.00, '2021-07-20');

INSERT INTO taxi_calls (client_name, client_phone, call_time, pickup_address, destination_address, status, price, duration, distance) VALUES
('Anna Ivanova', '+79991234567', NOW() - INTERVAL '15 minutes', 'Lenina St, 25', 'Pobedy Ave, 10', 'completed', 350.00, 25, 12.5),
('Mikhail Sidorov', '+79992345678', NOW() - INTERVAL '45 minutes', 'Gagarina St, 15', 'Domodedovo Airport', 'accepted', NULL, NULL, NULL);

-- Insert admin user
-- Password: admin123 (hashed using BCrypt)
INSERT INTO users (username, email, password_hash, role, first_name, last_name, phone, is_active) VALUES
('admin', 'admin@taxi.com', '$2a$11$8vTxWqV3b5Kz5vTxWqV3bOvTxWqV3b5Kz5vTxWqV3b5Kz5vTxWqV.', 'Admin', 'Admin', 'User', '+79990000000', true);

-- Success message
SELECT 'Database initialized successfully with admin user!' as message;