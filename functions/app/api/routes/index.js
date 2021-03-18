'use strict';

//--------------//
// Dependencies //
//--------------//
const router = require('express').Router();

module.exports = (app) => {
	//--------------------------------------------//
	// Attaching all Middlewares for this project //
	//--------------------------------------------//
	const middlewares = {
		common: require('./common/middleware')(app)
	};

	//------------------------------//
	// Attaching middlewares to app //
	//------------------------------//
	app.middlewares = middlewares;

	//------------------------------//
	// Attaching all the API routes //
	//------------------------------//
	const routes = {
		user: require('./user')(app)
	};

	//----------------------------------------//
	// Attaching protected routes validations //
	//----------------------------------------//
	router.use('/account*', middlewares.common.tokenValidator());

	/**
     * User Routes
     */
	router.use('/account/user', middlewares.common.checkToken(), routes.user.private);

	//-----------------------//
	// Attaching 404 handler //
	//-----------------------//

	router.all('*', (req, res) => {
		res.status(404).end();
	});

	return router;
};
