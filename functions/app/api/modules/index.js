'use strict';

module.exports = (app) => {
	const session = require('./session')(app);

	const user = require('./user')(app);

	return {
		session,
		user
	};
};
