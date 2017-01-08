package md5105beb2f4bcf428b4d1f3d455dad0736;


public class EditNotesPopup
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_dismiss:()V:GetDismissHandler\n" +
			"";
		mono.android.Runtime.register ("NawigatorB34.Android.EditNotesPopup, NawigatorB34.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EditNotesPopup.class, __md_methods);
	}


	public EditNotesPopup () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EditNotesPopup.class)
			mono.android.TypeManager.Activate ("NawigatorB34.Android.EditNotesPopup, NawigatorB34.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public EditNotesPopup (android.content.res.AssetManager p0, android.content.Context p1, int p2) throws java.lang.Throwable
	{
		super ();
		if (getClass () == EditNotesPopup.class)
			mono.android.TypeManager.Activate ("NawigatorB34.Android.EditNotesPopup, NawigatorB34.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Res.AssetManager, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void dismiss ()
	{
		n_dismiss ();
	}

	private native void n_dismiss ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
