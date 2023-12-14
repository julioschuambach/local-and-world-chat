# Multi Channel

## Introduction
This project consists of a prototype of a multi (and targeted) channel chat using .NET and RabbitMQ.

## Requirements
- .NET 6
- RabbitMQ running on ports 15672 and 5672

## Demo
![demo](/media/demo.gif)

## How to use

### Installing RabbitMQ via Docker
On terminal/prompt:
```cmd
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=Pass@123 rabbitmq:3-management
```

After running the container, you can access the RabbitMQ interface by the following link: `http://localhost:15672/`

### Restoring the project
`dotnet restore`

### Building the project
`dotnet build`

### Running the project
`dotnet run`
