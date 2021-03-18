'use strict';

module.exports = (app) => {
	const Courses = app.db.collection('Walden').doc('VR').collection('courses').where('published', '==', true);
	const Sections = app.db.collection('Walden').doc('VR').collection('coursesections').where('published', '==', true);
	const Assets = app.db.collection('Walden').doc('VR').collection('assets').where('published', '==', true);

	//---------------------------------//
	// Helper function for courses API //
	//---------------------------------//
	const getCourses = () => {
		return Courses.get()
			.then((docRef) => {
				const courses = [];
				docRef.forEach((doc) => {
					courses.push(doc.data());
				});
				return Promise.resolve(courses);
			})
			.catch(() => {
				return Promise.reject({ errCode: 'UNKNOWN_ERROR' });
			});
	};

	//---------------------------------//
	// Helper function for courses API //
	//---------------------------------//
	const getSections = () => {
		return Sections.get()
			.then((docRef) => {
				const sestions = [];
				docRef.forEach((doc) => {
					sestions.push(doc.data());
				});
				return Promise.resolve(sestions);
			})
			.catch(() => {
				return Promise.reject({ errCode: 'UNKNOWN_ERROR' });
			});
	};

	//---------------------------------//
	// Helper function for courses API //
	//---------------------------------//
	const getAssets = () => {
		return Assets.get()
			.then((docRef) => {
				const assets = [];
				docRef.forEach((doc) => {
					assets.push(doc.data());
				});
				return Promise.resolve(assets);
			})
			.catch(() => {
				return Promise.reject({ errCode: 'UNKNOWN_ERROR' });
			});
	};

	//---------------------------------//
	// Helper function for courses API //
	//---------------------------------//
	const getLinkedAssests = (req, _assets, section) => {
		return _assets
			.map((asset) => {
				if (section.assets.includes(asset.contentfulId)) {
					// Filter Applied
					if (req.query.assetType === undefined) {
						return asset;
					} else {
						if (req.query.assetType === asset.type) {
							return asset;
						}
					}
				}
			})
			.filter((item) => {
				// remove undefined elements from array
				if (item) {
					return true;
				}
			});
	};

	//  OPTIMIZED GetCourse API [res time reduce by 1min -> 3s]
	//  STEP TO RETRIVE THE COURSE INFO
	// 	1. Get all courses from courses collection
	// 	2. Get sections info from coursesections collection by course.sections
	// 	3. Get assets info from assets collection by section.assets

	const courses = (req) => {
		return Promise.all([ getCourses(), getSections(), getAssets() ]).spread((_courses, _sections, _assets) => {
			// Now map courses with sesctions and assets

			const result = _courses.map((courseItem) => {
				// doing shallow copy so that main array will remain same
				const course = { ...courseItem };
				const allAssets = [];
				if (course.sections) {
					// Get the linked section document data
					const sectionsArray = _sections
						.map((sectionItem) => {
							// doing shallow copy so that main array will remain same
							const section = { ...sectionItem };
							if (course.sections.includes(section.contentfulId)) {
								if (section.assets) {
									// Get the linked asset document data
									const asstesArray = getLinkedAssests(req, _assets, section);
									section.assets = asstesArray;
									allAssets.push(asstesArray);
								} else {
									section.assets = [];
								}
								return section;
							}
						})
						.filter((item) => {
							if (item) {
								return true;
							}
						});

					course.sections = sectionsArray;
				} else {
					course.sections = [];
				}
				delete course.assets;
				course.assets = allAssets.flatMap((item) => item).sort((a, b) => {
					if (a.title < b.title) {
						return -1;
					}
					if (a.title > b.title) {
						return 1;
					}
					return 0;
				});
				return course;
			});
			return Promise.resolve({
				content: result
			});
		});
	};
	return {
		get: courses
	};
};
