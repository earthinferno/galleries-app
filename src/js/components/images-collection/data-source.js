  import {baseapiurl} from '../../globals';
  
  class ImageDataService {

    static getImageData(callback) {
      $.get(baseapiurl + "/api/media", function(data, status){
        callback(data.results);
      });
    }

    static getImageData(data, callback) {
      $.get(baseapiurl + '/api/media?galleryId=' + data.galleryId + '&userId=' + data.userId, function(data, status){
        callback(data);
      });

    }

        
    static addImage(files, callback) {
      $.ajax({
        url: baseapiurl + '/api/media',
        data: files,
        cache: false,
        contentType: false,
        processData: false,
        method: 'Post',
        success:  function(data, status){
          callback(data);
        }
      });
    }
  }

  export { ImageDataService }