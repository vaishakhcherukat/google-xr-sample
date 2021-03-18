'use strict';

module.exports = (app) => {
	//-------------------------------------//
	// req body Schema for getUsersettings //
	//-------------------------------------//
	const getUsersettings = {
		email: {
			type: 'string',
			format: 'email',
			allowEmpty: false,
			required: true
		}
	};

	//-------------------------------------//
	// req body Schema for setUsersettings //
	//-------------------------------------//
	const setUsersettings = {
		autoDownloadOverWifi: {
			type: 'boolean',
			allowEmpty: false,
			required: true
		}
	};

	return {
		settings: {
			get: getUsersettings,
			set: setUsersettings
		}
	};
};
