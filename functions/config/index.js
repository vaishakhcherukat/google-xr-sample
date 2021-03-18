'use strict';

module.exports = () => {
	return {
		serviceAccount: {
			dev: require('./service-account/dev-env.json'),
			prod: require('./service-account/prod-env.json')
		}
	};
};
