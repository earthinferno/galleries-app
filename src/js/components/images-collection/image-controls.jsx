import React from 'react';

export default class ImageControls extends React.Component{

    render(){
        let context = this.props.context;

        const updateContent = 

            <div>
                <button className='btn btn-outline-primary' onClick={this.props.handledUpdate}>Update</button>
                <button className='btn btn-outline-primary' onClick={this.props.handledDelete}>Delete</button>
            </div>;

        const addContent = 
            <span>
                <button className='btn btn-outline-primary' onClick={this.props.handledAdd}>Add</button>
            </span>;
        
        return ( 
            <span>
                {context === 'ADD' && addContent }
                {context === 'UPDATE' && updateContent }
            </span>
        );
    };
}