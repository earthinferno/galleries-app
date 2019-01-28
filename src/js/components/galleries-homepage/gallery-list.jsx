import React from 'react';
import GallerySummary from './gallery-summary.jsx';

export default class GalleryList extends React.Component {
/*
    constructor(props) {
        super(props);
        this.state = {
          galleries : [],
        }

        this.refreshGalleryData = this.refreshGalleryData.bing(this);
   
      }
    
    componentDidMount() {
    GalleryDataService.getGalleriesData(this.props.UserId, galleryData => 
        this.setState({galleries: galleryData})
    );
    }
    
    refreshGalleryData() {
        GalleryDataService.getGalleriesData(this.props.UserId, galleryData => 
            this.setState({galleries: galleryData})
        );
    }*/
  
    render(){
        return (
            <div>
                <ul>
                {this.props.galleries.map( gallery => (
                    <GallerySummary key={gallery.id} data={gallery} />
                  ))}
                </ul>
            </div>
        );
    }
}
