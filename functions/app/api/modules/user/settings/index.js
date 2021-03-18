'use strict';

module.exports = (app) => {
	// Users model
	const Users = app.db.collection('Walden').doc('VR').collection('users');

	//-------------------//
	// Set User Settings //
	//-------------------//

	const setSettings = (req) => {
		return Users.doc(req.query.email).get().then(async (result) => {
			// if No User Found then
			// 1. Create a new document
			// 2. With new Settings Value

			if (result.data() === undefined) {
				try {
					await Users.doc(req.query.email).set({
						settings: {
							autoDownloadOverWifi: req.body.autoDownloadOverWifi
						}
					});
				} catch (e) {
					return Promise.reject({ errCode: 'UNKNOWN_ERROR', errDetails: e.message, statusCode: 400 });
				}
			}

			// IF user found
			// 1. Update the user settings value
			try {
				await Users.doc(req.query.email).update({
					settings: {
						autoDownloadOverWifi: req.body.autoDownloadOverWifi
					}
				});
			} catch (e) {
				return Promise.reject({ errCode: 'UNKNOWN_ERROR', errDetails: e.message, statusCode: 400 });
			}
			return Promise.resolve({ content: { autoDownloadOverWifi: req.body.autoDownloadOverWifi } });
		});
	};

	//-------------------//
	// Get User Settings //
	//-------------------//
	const getSettings = (queryParams) => {
		const email = queryParams.email;
		// default settings obj
		const defaultSettingsData = {
			settings: {
				autoDownloadOverWifi: true
			}
		};
		return Users.doc(email).get().then((result) => {
			if (result.data() === undefined) {
				// if no user found then send default data
				return Promise.resolve({ content: defaultSettingsData });
			}
			// send settings data
			return Promise.resolve({ content: result.data().settings ? result.data().settings : defaultSettingsData });
		});
	};
	return {
		set: setSettings,
		get: getSettings
	};
};
