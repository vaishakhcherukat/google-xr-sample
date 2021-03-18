'use strict';

const expressRouter = require('express').Router;

module.exports = (app) => {
	/**
     * Route components
     */
	const routes = {
		courses: require('./courses/route'),
		settings: require('./settings/route')
	};

	//---------------//
	// Public Routes //
	//---------------//
	const publicRoutes = expressRouter();

	//----------------//
	// Private Routes //
	//----------------//
	const privateRoutes = expressRouter();

	privateRoutes.use('/courses', routes.courses(app));
	privateRoutes.use('/settings', routes.settings(app));

	return {
		public: publicRoutes,
		private: privateRoutes
	};
};
