# FlightControlWeb

Homework assignment for CS degree 2nd year.

# Unit Tests
1. Verify add flight action working properly - using mock
2. Verify delete flight action working properly - using mock
3. Verify get flight action working properly - using stub
4. Verify add server action working properly - using mock
5. Verify delete server action working properly - using mock
6. Verify get server action working properly - using stub


# Notes
* We did the client side in ReactJS, the source files are located in a directory named FlightControlWeb\ReactClient

** The client side is currently working with the server on https://localhost:5001
To change the server we are working with just edit the wwwroot\index.html file, where you'll find a global var named: 'window.SERVER_URL'


*** There are 3 known JS console errors in the imported 'google-maps-react' package we used:

- Warning: Using UNSAFE_componentWillReceiveProps in strict mode is not recommended...
- Warning: Can't call setState on a component that is not yet mounted. This is a no-op, ...
- InvalidValueError: setCenter: not a LatLng or LatLngLiteral with finite coordinates: in property lat: NaN is not an accepted value

^ This last error may seem like it's originated in our code yet we verified that we send a valid number variable using parseFloat() method, not only that in our map Marker LatLng creation the very same variables do not generate such error. Concluding, this is also an error originated from the 'google-maps-react' package we have used which is only a wrapper to the original google maps package and probably includes some implementation bugs.
