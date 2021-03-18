'use strict';

module.exports = (app) => {
	const validateSession = (authToken) => {
		if (authToken === 'f8428bcfc4041ae2d5ebde0c51d53c80332dda14c8a34c5769b278296c7e14aa') {
			return Promise.resolve();
		} else {
			return Promise.reject({
				errCode: 'INVALID_AUTH_TOKEN',
				statusCode: 401,
				errDetails: 'Unauthorized Access! Invalid auth token.'
			});
		}
	};

	return {
		validateSession
	};
};
