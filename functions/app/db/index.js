'use strict';

//--------------//
// Dependencies //
//--------------//
const admin = require('firebase-admin');
const functions = require('firebase-functions');

//---------------------//
// Requiring DB Config //
//---------------------//
const dbConfig = require('./config');

//---------------------//
// Firebase ENV CONFIG //
//---------------------//
let FIREBASE_ENV;

if (functions.config().project.env) {
	FIREBASE_ENV = functions.config().project.env;
} else {
	throw new Error('Function Config Error: project.env is undefined');
}
console.log(`Firebase env config is set to: ${FIREBASE_ENV} environment`);

module.exports = (app) => {
	let db = {};
	try {
		//--------------------------//
		// Intializing Firebase app //
		//--------------------------//
		if (FIREBASE_ENV === 'dev') {
			admin.initializeApp({
				credential: admin.credential.cert(app.config.serviceAccount.dev),
				databaseURL: dbConfig.databaseURL.dev
			});
		} else {
			admin.initializeApp({
				credential: admin.credential.cert(app.config.serviceAccount.prod),
				databaseURL: dbConfig.databaseURL.prod
			});
		}

		//-----------------------------//
		// Intializing firestore as db //
		//-----------------------------//
		db = admin.firestore();

		//-------------------------//
		// Ataching dbConfig to db //
		//-------------------------//
		db.config = dbConfig;
	} catch (e) {
		console.log('Error in firebase intialization. => ', e);
	}

	return db;
};
