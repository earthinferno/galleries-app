  import {baseapiurl} from '../../globals';
  
  class Auth0DataService {

    static getAccount(userid, callback) {
      $.get(baseapiurl + "/api/accounts/" + userid, function(data, status){
        callback(data);
      });
    }

    // static getAccount(userId, callback, callbackFailure){
    //     $.ajax({
    //       url: baseapiurl + '/api/accounts/' + userId,
    //       type: 'GET',
    //       success:  function(data, status){
    //         callback(data);
    //       },
    //       error: function (request, status, error){
    //         callbackFailure(request, status, error);
    //       }
    //     });
    // }
  }

  export { Auth0DataService }