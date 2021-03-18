'use strict';

module.exports = (app) => {
	const errorCodes = require('./scripts/errorCode')(app);

	const getErrorCode = (errCode) => {
		const errorCode = errorCodes[errCode];

		if (!errorCode) {
			console.log(`${errCode} => doesn't have error code`);
			if (app.get('env') !== 'production') {
				return errCode;
			}
		}
		return errorCode || 403;
	};

	const successHandler = (req, res, next) => {
		if (req.workflow.outcome.data) {
			res.status(200).json({
				success: true,
				data: req.workflow.outcome.data
			});
		} else {
			res.status(400).end();
		}
	};

	const errorHandler = (err, req, res, next) => {
		if (typeof err === 'object' && err.hasOwnProperty('errCode') && typeof err.errCode === 'string') {
			const response = {
				success: false,
				errorCode: getErrorCode(err.errCode),
				statusCode: err.statusCode
			};

			if (err.hasOwnProperty('errDetails')) {
				response.errorDetails = err.errDetails;
			}

			if (app.get('env') !== 'production') {
				console.log(err);
				if (!req.workflow) {
					return next(err);
				}
				if (req.workflow.outcome.errors.length) {
					response.errors = req.workflow.outcome.errors;
				}

				if (Object.keys(req.workflow.outcome.errFor).length) {
					response.errFor = req.workflow.outcome.errFor;
				}

				if (typeof response.errorCode === 'string') {
					response.error_code = [ response.errorCode ];
					response.errorCode = 403;
				}
			}
			return res
				.status(response.errorCode ? (response.statusCode ? response.statusCode : 200) : 500)
				.json(response);
		} else {
			return next(err);
		}
	};

	return [ successHandler, errorHandler ];
};
