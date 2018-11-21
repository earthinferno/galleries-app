import React from 'react'
import {render} from 'react-dom'
import { Provider } from 'react-redux'
import { createStore } from 'redux';
import rootReducer from './reducers';
import App from './components/app.jsx'
//import App from './app.jsx';
import './../css/main.scss';

const store = createStore(rootReducer)

render(
    <Provider store ={store}>
      <App/>
    </Provider>,
    document.getElementById('app-root')
)
