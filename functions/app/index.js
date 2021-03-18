'use strict';

//--------------//
// Dependencies //
//--------------//
const express = require('express');
const bodyParser = require('body-parser');
const helmet = require('helmet');
const compression = require('compression');

//------------------//
// Attaching config //
//------------------//
const config = require('../config')();

//-------------------------//
// Intializing Express app //
//-------------------------//
const app = express();

//----------------------//
// Attaching bodyParser //
//----------------------//
app.use(bodyParser.json());

//----------------------------//
// Attaching gzip compression //
//----------------------------//
app.use(compression());

//-------------------------//
// Attaching helmet to app //
//-------------------------//
app.use(helmet());

//------------------//
// Express Settings //
//------------------//
app.disable('x-powered-by');

//-----------------------------//
// Promise configuration setup //
//-----------------------------//
global.Promise = require('bluebird');
if (app.get('env') === 'production') {
	Promise.config({
		warnings: false,
		longStackTraces: false,
		cancellation: false,
		monitoring: false
	});
} else {
	Promise.config({
		warnings: false,
		longStackTraces: true,
		cancellation: true,
		monitoring: true
	});
}

//-----------------------//
// Attaching cors module //
//-----------------------//
app.use(
	require('cors')({
		origin: '*',
		methods: [ 'PUT', 'GET', 'POST', 'DELETE', 'PATCH', 'OPTIONS' ],
		allowedHeaders: [ 'accept', 'content-type', 'x-auth-token' ],
		preflightContinue: false,
		optionsSuccessStatus: 204
	})
);

//-----------------------------//
// Attaching config to the app //
//-----------------------------//
app.config = config;

//--------------------------//
// Attaching Utility to app //
//--------------------------//
app.utility = require('./utility')(app);

//-------------------------------//
// Attaching the workflow module //
//-------------------------------//
app.use(app.utility.attachWorkflow);

//--------------//
// Attaching db //
//--------------//
const db = require('./db')(app);

//---------------------//
// Attaching db to app //
//---------------------//
app.db = db;

//-------------------//
// Attaching modules //
//-------------------//
app.module = require('./api/modules')(app);

app.use('/api/v1', [
	//------------------//
	// Attaching Routes //
	//------------------//
	require('./api/routes')(app),

	//----------------------------//
	// Attaching response handler //
	//----------------------------//
	require('./api/responseHandler')(app)
]);

//---------------------//
// Default 400 handler //
//---------------------//
app.use((req, res) => {
	res.status(400).end();
});

require('events').EventEmitter.prototype._maxListeners = 100;

//////////////////////////////////
// ONLY FOR DEVELOPMENT PURPOSE //
//////////////////////////////////

if (process.env.NODE_ENV) {
	app.set('env', process.env.NODE_ENV);
}
console.log(`Project is running in "${app.get('env')}" mode.`);

if (app.get('env') !== 'production') {
	app.listen(8000, () => {
		console.log('Server started at port 8000');
	});
}

module.exports = app;
