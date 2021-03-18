'use strict';

//--------------//
// Dependencies //
//--------------//
const functions = require('firebase-functions');

//-----------------------//
// Attaching Express App //
//-----------------------//
const app = require('./app');

exports.vr = functions.https.onRequest(app);
