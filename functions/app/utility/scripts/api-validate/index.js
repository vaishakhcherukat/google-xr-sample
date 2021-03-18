'use strict';

const reval = require('revalidator');

/**
 * Will validate schema
 * @param {Object} schema 
 */
const allValidate = (schema) => {
	return (req, res, next) => {
		const queue = [];

		// headers
		if (schema.hasOwnProperty('headers')) {
			queue.push({
				schema: {
					properties: schema.headers
				},
				value: req.headers,
				where: 'headers'
			});
		}

		// params
		if (schema.hasOwnProperty('params')) {
			queue.push({
				schema: {
					properties: schema.params
				},
				value: req.params,
				where: 'params'
			});
		}

		// query
		if (schema.hasOwnProperty('query')) {
			queue.push({
				schema: {
					properties: schema.query
				},
				value: req.query,
				where: 'query'
			});
		}

		// body
		if (schema.hasOwnProperty('body')) {
			queue.push({
				schema: {
					properties: schema.body
				},
				value: req.body,
				where: 'body'
			});
		}

		const output = queue
			.map((e) => {
				const result = reval.validate(e.value, e.schema);
				if (!result.valid) {
					result.errors.forEach((error) => (error.where = e.where));
				}
				return result;
			})
			.reduce(
				(acc, val) => {
					return {
						valid: acc.valid && val.valid,
						errors: acc.errors.concat(val.errors)
					};
				},
				{
					valid: true,
					errors: []
				}
			);
		if (!output.valid) {
			req.workflow.outcome.errors = output.errors.map((e) => {
				return `${e.property} ${e.message} in ${e.where}`;
			});
			return next({ errCode: 'API_VALIDATION_ERROR', statusCode: 400 });
		} else {
			return next();
		}
	};
};

const headerValidate = (schema) => {
	return allValidate({ headers: schema });
};

const bodyValidate = (schema) => {
	return allValidate({ body: schema });
};

const paramsValidate = (schema) => {
	return allValidate({ params: schema });
};

const queryValidate = (schema) => {
	return allValidate({ query: schema });
};

module.exports = {
	headers: headerValidate,
	body: bodyValidate,
	params: paramsValidate,
	query: queryValidate
};
