'use strict';

module.exports = (app) => {
	/**
     * Attaching all utilities
     */
	const utility = {
		apiValidate: require('./scripts/api-validate'),
		workflow: require('./scripts/workFlow')
	};

	utility.attachWorkflow = (req, res, next) => {
		req.workflow = req.app.utility.workflow(req, res);
		next();
	};
	return utility;
};
