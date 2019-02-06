import React from 'react'
import ReactDOM from 'react-dom';
import App from './components/app.jsx';
import { Router } from 'react-router-dom';
import 'bootstrap';
import './../css/main.scss';
import './config/web.config';
import createBrowserHistory from "history/createBrowserHistory";

const history = createBrowserHistory();

ReactDOM.render((
    <Router history={history}>
        <App  />
    </Router>
    ),
    document.getElementById('app-root')
);

