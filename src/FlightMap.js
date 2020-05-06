import {Map, Polyline, Marker, GoogleApiWrapper} from 'google-maps-react';
import Table from "react-bootstrap/Table";
import * as React from "react";

const apiKey = 'AIzaSyAjngpsKv9PcK9NqXrHi8VdNi_5VI287CM';

let myLatLng = {lat: -25.363, lng: 131.044};
let flightClicked;
let flightsList;

export class FlightMap extends React.Component  {
    render() {
        return (
            <Map google={this.props.google} style={containerStyle}
                           containerStyle={containerStyle} zoom={14} center={myLatLng}>
                <Marker onClick={onAirplaneClick()}
                        icon={{
                    url: "./resources/pngwave.png",
                    position: {myLatLng},
                    scaledSize: new this.props.google.maps.Size(64,64),
                    origin: new this.props.google.maps.Point(0, 0),
                    anchor: new this.props.google.maps.Point(0, 32) }}
                        name={'Airplane'} />
            </Map>
        );
    }
}
//
// const generatePolylines = ({flightClicked}) => {
//     let items = [];
//
//     for (let [key, value] of Object.entries(flightsList)) {
//         let points = [{lat: 25.774, lng: -80.190},
//             {lat: 25.774, lng: -80.190}];
//         items.push(
//             <Polyline
//                 path={points}
//                 strokeColor={flightClicked?"#0000FF":"#FFFF00"}
//                 strokeOpacity={flightClicked?1:0.7}
//                 strokeWeight={flightClicked?4:2} />
//         )
//     }
//
//     return (items);
// };
//
// const generatePolylines = ({flight,flightClicked}) => {
//     let items = [];
//     let onClick;
//
//     for (let [key, value] of Object.entries(flightsList)) {
//         let points = [{lat: 25.774, lng: -80.190},
//                         {lat: 25.774, lng: -80.190}];
//         items.push(
//             <Polyline
//                 path={points}
//                 strokeColor={flightClicked?"#0000FF":"#FFFF00"}
//                 strokeOpacity={flightClicked?1:0.7}
//                 strokeWeight={flightClicked?4:2} />
//         )
//     }
//
//     return (items);
// };

const onAirplaneClick = (e) => {
    flightClicked = e;
}

const containerStyle = {
    position: 'relative',
    width: '500px',
    height: '400px'
}

export default GoogleApiWrapper({
    apiKey: apiKey
})(FlightMap)