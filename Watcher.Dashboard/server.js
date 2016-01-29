var http = require('http');
var port = process.env.port || 1337;
var url = require('url');
var redis = require('redis');

http.createServer(function (req, res) {
    var queryObject = url.parse(req.url, true).query;
    var event = queryObject.event;
    var period = queryObject.period;
    if (event == undefined || period == undefined) {
        res.writeHead(400, { 'Content-Type': 'text/plain' });
        res.end("Bad Request.");
    }
    else {
        var client = redis.createClient(6379, 'localhost');
        client.hgetall(event + '_' + period, function (err, object) {
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(object));
        });
    }
}).listen(port);