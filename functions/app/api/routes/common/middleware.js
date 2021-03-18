'use strict';

module.exports = (app) => {
	//-------------------------------------------------------//
	// Validates the incmoing API header for required fields //
	//-------------------------------------------------------//
	const headerValidator = () => {
		/**
         * Default Header Schema
         */
		const headerSchema = {
			'x-auth-device-id': {
				type: 'string',
				required: true
			}
		};

		return app.utility.apiValidate.headers(headerSchema);
	};

	const tokenValidator = () => {
		/**
		 * Additional header schema for /account* route
		 */
		const tokenHeaderSchema = {
			'x-auth-token': {
				type: 'string',
				required: true,
				allowEmpty: false
			}
		};
		return app.utility.apiValidate.headers(tokenHeaderSchema);
	};

	const checkToken = () => {
		return (req, res, next) => {
			return app.module.session
				.validateSession(req.headers['x-auth-token'])
				.then(() => {
					return next();
				})
				.catch(next);
		};
	};
	return {
		headerValidator,
		tokenValidator,
		checkToken
	};
};
