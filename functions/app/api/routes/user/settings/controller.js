'use strict';

module.exports = (app) => {
	// user module
	const user = app.module.user;

	//----------------------------//
	// getUserSettings Controller //
	//----------------------------//
	const getUserSettings = (req, res, next) => {
		user.settings
			.get(req.query)
			.then((output) => {
				req.workflow.outcome.data = output.content;
				req.workflow.outcome.statusCode = output.statusCode || 200;
				req.workflow.emit('response');
				return;
			})
			.catch(next);
	};

	//----------------------------//
	// setUserSettings Controller //
	//----------------------------//
	const setUserSettings = (req, res, next) => {
		user.settings
			.set(req)
			.then((output) => {
				req.workflow.outcome.data = output.content;
				req.workflow.outcome.statusCode = output.statusCode || 200;
				req.workflow.emit('response');
				return;
			})
			.catch(next);
	};

	return { getUserSettings, setUserSettings };
};
