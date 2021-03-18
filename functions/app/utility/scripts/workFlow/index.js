'use strict';

module.exports = (req, res) => {
	const workflow = new (require('events')).EventEmitter();

	workflow.outcome = {
		success: false,
		errors: [],
		errFor: {},
		message: '',
		data: undefined,
		statusCode: 200
	};

	workflow.setMaxListeners(0);

	workflow.hasErrors = () => {
		return Object.keys(workflow.outcome.errFor).length !== 0 || workflow.outcome.errors.length !== 0;
	};

	workflow.on('eception', (error) => {
		try {
			if (typeof error === 'object' && !(error instanceof Error)) {
				error = JSON.parse(JSON.stringify(error));
				workflow.outcome.errFor = error;
			} else {
				workflow.outcome.errors.push(`Exception ${error}`);
			}
		} catch (e) {
			workflow.outcome.errors.push(`Exception ${error}`);
		} finally {
			workflow.emit('response');
		}
	});

	workflow.on('response', () => {
		workflow.outcome.success = !workflow.hasErrors();

		if (!workflow.outcome.success) {
			delete workflow.outcome.data;
			delete workflow.outcome.message;
		} else {
			delete workflow.outcome.errors;
			delete workflow.outcome.errFor;
			if (!workflow.outcome.message) {
				delete workflow.outcome.message;
			}
			if (req.method === 'DELETE') {
				delete workflow.outcome.data;
			}
		}

		delete workflow.outcome.message;
		res.status(workflow.outcome.statusCode).send(workflow.outcome);
	});
	return workflow;
};
