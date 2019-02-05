import React from 'react';
import GallerySummary from './gallery-summary.jsx';

export default class GalleryList extends React.Component {
  
    render(){
        return (
            <div className='row'>
                {/* <ul> */}
                {this.props.galleries.map( gallery => (
                    <GallerySummary key={gallery.id} data={gallery} userId={this.props.userId} {...this.props} />
                  ))}
                {/* </ul> */}
            </div>
        );
    }
}
