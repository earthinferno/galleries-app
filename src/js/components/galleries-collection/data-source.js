  import {baseapiurl} from '../../globals';
  
  class GalleryDataService {
    static getGalleriesData(userId, callback){
      $.get(baseapiurl + "/api/galleries/" + userId, function(data, status){
        callback(data);
      });      
    }

    static addGallery(data, callback){
      $.ajax({
        url: baseapiurl + '/api/galleries',
        dataType: 'json',
        method: 'Post',
        contentType: 'application/json',
        cache: false,
        data: JSON.stringify(data),
        processData: false,
        success:  function(data, status){
          callback(data);
        }
      });
    } 
  
}

  export { GalleryDataService }