import React from 'react'
//import {render} from 'react-dom'
import ReactDOM from 'react-dom';
import App from './components/app.jsx';
import { Router } from 'react-router-dom';
import './../css/main.scss';
import './config/web.config';
import createBrowserHistory from "history/createBrowserHistory";

const history = createBrowserHistory();


/*
ReactDOM.render(
    <App/>,
    document.getElementById('app-root')
)
*/

ReactDOM.render((
    <Router history={history}>
        <App history={history}/>
    </Router>
    ),
    document.getElementById('app-root')
);

