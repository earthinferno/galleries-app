import React from 'react'
//import {render} from 'react-dom'
import ReactDOM from 'react-dom';
import App from './components/app.jsx'
import './../css/main.scss';
import './config/web.config';

ReactDOM.render(
    <App/>,
    document.getElementById('app-root')
)
