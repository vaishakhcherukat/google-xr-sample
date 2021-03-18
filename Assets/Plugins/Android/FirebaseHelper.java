package net.rapidvalue.laureate.vr;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.view.View;
import android.app.Activity;
import android.R.style;
import com.unity3d.player.UnityPlayer;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.OAuthProvider;
import com.google.firebase.auth.OAuthProvider.Builder;
import com.google.firebase.auth.FirebaseAuthException;
import com.google.android.gms.tasks.Task;
import com.google.android.gms.tasks.OnFailureListener;

  
class FirebaseHelper
{
    public static void getCredential()
    {
      FirebaseAuth firebaseAuth = FirebaseAuth.getInstance();
        OAuthProvider.Builder provider = OAuthProvider.newBuilder("microsoft.com");
        provider.addCustomParameter("prompt", "select_account");
        provider.addCustomParameter("tenant", "common");
        
        Task<AuthResult> pendingResultTask = firebaseAuth.getPendingAuthResult();
        if (pendingResultTask != null) {
          pendingResultTask.addOnFailureListener(
          new OnFailureListener() {
            @Override
            public void onFailure(Exception e) {
              displaySignInError(e);
            }
          });
        } else {
          firebaseAuth
              .startActivityForSignInWithProvider(UnityPlayer.currentActivity, provider.build())
              .addOnFailureListener(
              new OnFailureListener() {
                @Override
                public void onFailure(Exception e) {
                  displaySignInError(e);
                }
              });
        }
    }

    public static void displaySignInError(Exception e) {
      if (e instanceof FirebaseAuthException) {
        
        if (((FirebaseAuthException) e).getErrorCode() == "ERROR_WEB_CONTEXT_CANCELED") {
          displaySignInError("You must sign in to continue.");
          return;
        }
      }
      displaySignInError("We're sorry. Something went wrong, and we couldn't sign you in. Please try again later.");
    }

    public static void displaySignInError(String message) {
      AlertDialog.Builder builder = new AlertDialog.Builder(UnityPlayer.currentActivity, style.Theme_DeviceDefault_Light_Dialog_Alert);
      builder.setTitle("Sign In");
      builder.setMessage(message);
      builder.setCancelable(false);

      builder.setPositiveButton(
        "OK",
        new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                FirebaseHelper.getCredential();
            }
        }
      );
      builder.create().show();
    }
}
