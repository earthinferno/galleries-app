  import {baseapiurl} from '../../globals';
  
  class RegisterDataService {

    static registerUser(userId, data, callback){
      $.ajax({
        url: baseapiurl + '/api/register',
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

  export { RegisterDataService }