'use strict';

//------------------------------------------------//
// THIS IS THE ROUTE FILE FOR USER/COURSES MODULE //
//------------------------------------------------//

const router = require('express').Router();

module.exports = (app) => {
	//--------------------------------------//
	// Attaching controller for this module //
	//--------------------------------------//
	const controller = require('./controller')(app);

	//----------------------------//
	// Attaching schema validator //
	//----------------------------//
	const schemaValidator = require('./schemaValidator')(app);
	/**
	 * GET [Courses]
	 */
	router.get('/', [ app.utility.apiValidate.query(schemaValidator.courses), controller.getCourses ]);
	return router;
};
