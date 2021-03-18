'use strict';

module.exports = (app) => {
	const courses = require('./courses')(app);
	const settings = require('./settings')(app);

	return {
		courses,
		settings
	};
};
