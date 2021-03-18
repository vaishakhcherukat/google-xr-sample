'use strict';

//-------------------------------------------------//
// THIS IS THE ROUTE FILE FOR USER/SETTINGS MODULE //
//-------------------------------------------------//

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
	 * GET [SETTINGS]
	 */
	router.get('/', [ app.utility.apiValidate.query(schemaValidator.settings.get), controller.getUserSettings ]);

	router.post('/', [
		app.utility.apiValidate.query(schemaValidator.settings.get),
		app.utility.apiValidate.body(schemaValidator.settings.set),
		controller.setUserSettings
	]);

	return router;
};
