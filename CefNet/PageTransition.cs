using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Types of transitions between pages. These are stored in the history
	/// database to separate visits, and are reported by the renderer for page
	/// navigations.
	/// </summary>
	/// <remarks>
	/// See https://cs.chromium.org/chromium/src/ui/base/page_transition_types.h
	/// </remarks>
	public enum PageTransition : uint
	{
		/// <summary>
		/// User got to this page by clicking a link on another page.
		/// </summary>
		Link = 0,

		/// <summary>
		/// User got this page by typing the URL in the URL bar.  This should not be
		/// used for cases where the user selected a choice that didn't look at all
		/// like a URL; see GENERATED below.
		///
		/// We also use this for other "explicit" navigation actions.
		/// </summary>
		Typed = 1,

		/// <summary>
		/// User got to this page through a suggestion in the UI, for example)
		/// through the destinations page.
		/// </summary>
		AutoBookmark = 2,

		/// <summary>
		/// This is a subframe navigation. This is any content that is automatically
		/// loaded in a non-toplevel frame. For example, if a page consists of
		/// several frames containing ads, those ad URLs will have this transition
		/// type. The user may not even realize the content in these pages is a
		/// separate frame, so may not care about the URL (see MANUAL below).
		/// </summary>
		AutoSubframe = 3,

		/// <summary>
		/// For subframe navigations that are explicitly requested by the user and
		/// generate new navigation entries in the back/forward list. These are
		/// probably more important than frames that were automatically loaded in
		/// the background because the user probably cares about the fact that this
		/// link was loaded.
		/// </summary>
		ManualSubframe = 4,

		/// <summary>
		/// User got to this page by typing in the URL bar and selecting an entry
		/// that did not look like a URL.  For example, a match might have the URL
		/// of a Google search result page, but appear like "Search Google for ...".
		/// These are not quite the same as TYPED navigations because the user
		/// didn't type or see the destination URL.
		/// See also KEYWORD.
		/// </summary>
		Generated = 5,

		/// <summary>
		/// This is a toplevel navigation. This is any content that is automatically
		/// loaded in a toplevel frame.  For example, opening a tab to show the ASH
		/// screen saver, opening the devtools window, opening the NTP after the safe
		/// browsing warning, opening web-based dialog boxes are examples of
		/// AUTO_TOPLEVEL navigations.
		/// </summary>
		AutoToplevel = 6,

		/// <summary>
		/// The user filled out values in a form and submitted it. NOTE that in
		/// some situations submitting a form does not result in this transition
		/// type. This can happen if the form uses script to submit the contents.
		/// </summary>
		FormSubmit = 7,

		/// <summary>
		/// The user "reloaded" the page, either by hitting the reload button or by
		/// hitting enter in the address bar.  NOTE: This is distinct from the
		/// concept of whether a particular load uses "reload semantics" (i.e.
		/// bypasses cached data).  For this reason, lots of code needs to pass
		/// around the concept of whether a load should be treated as a "reload"
		/// separately from their tracking of this transition type, which is mainly
		/// used for proper scoring for consumers who care about how frequently a
		/// user typed/visited a particular URL.
		///
		/// SessionRestore and undo tab close use this transition type too.
		/// </summary>
		Reload = 8,

		/// <summary>
		/// The url was generated from a replaceable keyword other than the default
		/// search provider. If the user types a keyword (which also applies to
		/// tab-to-search) in the omnibox this qualifier is applied to the transition
		/// type of the generated url. TemplateURLModel then may generate an
		/// additional visit with a transition type of KEYWORD_GENERATED against the
		/// url 'http://' + keyword. For example, if you do a tab-to-search against
		/// wikipedia the generated url has a transition qualifer of KEYWORD, and
		/// TemplateURLModel generates a visit for 'wikipedia.org' with a transition
		/// type of KEYWORD_GENERATED.
		/// </summary>
		Keyword = 9,

		/// <summary>
		/// Corresponds to a visit generated for a keyword. See description of
		/// KEYWORD for more details.
		/// </summary>
		KeywordGenerated = 10,

		/// <summary>
		/// ADDING NEW CORE VALUE? Be sure to update the LAST_CORE and CORE_MASK
		/// values below.  Also update CoreTransitionString().
		/// </summary>
		LastCore = KeywordGenerated,
		CoreMask = 0xFF,

		// Qualifiers
		// Any of the core values above can be augmented by one or more qualifiers.
		// These qualifiers further define the transition.

		/// <summary>
		/// A managed user attempted to visit a URL but was blocked.
		/// </summary>
		Blocked = 0x00800000,

		/// <summary>
		/// User used the Forward or Back button to navigate among browsing history.
		/// </summary>
		ForwardBack = 0x01000000,

		/// <summary>
		/// User used the address bar to trigger this navigation.
		/// </summary>
		FromAddressBar = 0x02000000,

		/// <summary>
		/// User is navigating to the home page.
		/// </summary>
		HomePage = 0x04000000,

		/// <summary>
		/// The transition originated from an external application; the exact
		/// definition of this is embedder dependent.
		/// </summary>
		FromApi = 0x08000000,

		/// <summary>
		/// The beginning of a navigation chain.
		/// </summary>
		ChainStart = 0x10000000,

		/// <summary>
		/// The last transition in a redirect chain.
		/// </summary>
		ChainEnd = 0x20000000,

		/// <summary>
		/// Redirects caused by JavaScript or a meta refresh tag on the page.
		/// </summary>
		ClientRedirect = 0x40000000,

		/// <summary>
		/// Redirects sent from the server by HTTP headers. It might be nice to
		/// break this out into 2 types in the future, permanent or temporary, if we
		/// can get that information from WebKit.
		/// </summary>
		ServerRedirect = 0x80000000,

		/// <summary>
		/// Used to test whether a transition involves a redirect.
		/// </summary>
		IsRedirectMask = 0xC0000000,

		/// <summary>
		/// General mask defining the bits used for the qualifiers.
		/// </summary>
		QualifierMask = 0xFFFFFF00,
	};
}
