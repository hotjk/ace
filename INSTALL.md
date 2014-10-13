# Grit.Demo.CQRS #

### Setup RabbitMQ ###

* Install RabblitMQ
* Start RabbitMQ service
* Add vhost/user/permission

>rabbitmqctl add_vhost grit

>rabbitmqctl add_user event_user event_password

>rabbitmqctl set_permissions -p grit event_user ".*" ".*" ".*"

>rabbitmqctl set_permissions -p grit guest ".*" ".*" ".*"

>rabbitmqctl set_user_tags event_user administrator

### Setup MySQL ###

* Install MySQL
* Import Dump20140707.sql

### Run ###

* CQRS.Demo.Sagas
* CQRS.Demo.EventConsumer
* CQRS.Demo.Web