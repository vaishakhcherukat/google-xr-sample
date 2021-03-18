'use strict';

module.exports = (app) => {
	const courses = {
		assetType: {
			type: 'string',
			allowEmpty: false
		},
		upn: {
			type: 'string',
			format: 'email',
			allowEmpty: false
		}
	};

	return {
		courses
	};
};
