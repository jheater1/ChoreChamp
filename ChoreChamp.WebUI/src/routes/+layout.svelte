<script lang="ts">
	import '../app.css';
	import { AppSidebar, Sidebar } from '$lib/components';

	let { children } = $props();
</script>

<Sidebar.Provider>
	<AppSidebar />
	<main>
		<Sidebar.Trigger />
		{@render children?.()}
	</main>
</Sidebar.Provider>
