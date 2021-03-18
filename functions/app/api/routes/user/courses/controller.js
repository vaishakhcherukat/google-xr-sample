'use strict';

module.exports = (app) => {
	const user = app.module.user;

	const getCourses = (req, res, next) => {
		user.courses
			.get(req)
			.then((output) => {
				req.workflow.outcome.data = output.content;
				req.workflow.outcome.statusCode = output.statusCode || 200;
				req.workflow.emit('response');
				return;
			})
			.catch(next);
	};

	return { getCourses };
};
